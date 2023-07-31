using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Helpers;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mgm.VI.Aws.Sqs;
using Mgm.VI.Aws;
using Mgm.VI.Logger;

namespace Mgm.VI.Aws.Sqs
{
    public class QueueAccess : IQueueAccess
    {
        #region Private vars
        private bool disposed = false;
        private bool suppressExceptions = true;
        private ReceiveMessageRequest receiveMessageRequest;
        private ReceiveMessageResponse receiveMessageResponse;
        private readonly int WaitTimeSeconds = 20;
        private readonly int VisibilityTimeout = 30;
        private AmazonSQSClient _sqsClient;
        private ILoggerService Logger;
        #endregion region Private vars

        #region Properties
        public AmazonSQSConfig SqsConfig { get; private set; } //AmazonSQSConfig
        public IAwsUser AwsUser { get; private set; }
        public bool SuppressExceptions
        {
            get
            {
                return suppressExceptions;
            }
            set
            {
                suppressExceptions = value;
            }
        }
        public Message CurrentMessage { get; private set; }
        public Message CurrentDeadLetter { get; private set; }
        public List<Message> FetchedMessages { get; private set; }
        public List<Message> DeadLetterMessages { get; private set; }
        public IErrorMessages ErrorMessages { get; private set; }
        public ISqsQueueDto MessageQueue { get; private set; }
        public ISqsQueueDto DeadLetterQueue { get; private set; }
        public AmazonSQSClient SqsClient
        {
            get
            {
                return _sqsClient;// ?? InstantiateSqsClient(AwsUser, SqsConfig);
            }
            private set 
            { 
                _sqsClient = value; 
            }
        }
        public string MessageGroupId { get; set; }
        public string DeadMessageGroupId { get; set; }
        public bool QueueWithMessageGroupId { get; private set; }
        public IRulesInspector RulesInspector { get; set; }
        #endregion Properties

        #region Constructors
        public QueueAccess(IAwsUser awsUser,
                            AmazonSQSConfig sqsConfig,
                            ISqsQueueDto messageQueue,
                            ISqsQueueDto deadLetterQueue,
                            bool queueWithMessageGroupId,
                            string messageGroupId,
                            ILoggerService logger,
                            IRulesInspector rulesInspector = null)
        {
            Initialize(awsUser, sqsConfig, messageQueue, deadLetterQueue, queueWithMessageGroupId, messageGroupId, logger, rulesInspector);
        }

        private void Initialize(    IAwsUser awsUser, 
                                    AmazonSQSConfig sqsConfig, 
                                    ISqsQueueDto messageQueue, 
                                    ISqsQueueDto deadLetterQueue, 
                                    bool queueWithMessageGroupId, 
                                    string messageGroupId, 
                                    ILoggerService logger,
                                   IRulesInspector rulesInspector)
        {
            if (ErrorMessages == null) ErrorMessages = new ErrorMessages();
            this.Logger = logger;
            this.SqsConfig = sqsConfig;
            this.AwsUser = awsUser;
            this.MessageQueue = messageQueue;
            this.DeadLetterQueue = deadLetterQueue;
            this.RulesInspector = rulesInspector;
            this.QueueWithMessageGroupId = queueWithMessageGroupId;
            if (QueueWithMessageGroupId)
            {
                MessageGroupId = string.IsNullOrEmpty(messageGroupId) ? Guid.NewGuid().ToString() : messageGroupId;
            }
            else
            {
                MessageGroupId = Guid.NewGuid().ToString();
            }

            try
            {
                InstantiateSqsClient(AwsUser, SqsConfig, false);
                if (SqsClient != null)
                {
                    DeadMessageGroupId = MessageGroupId;
                    if (FetchedMessages == null) FetchedMessages = new List<Message>();
                    if (DeadLetterMessages == null) DeadLetterMessages = new List<Message>();
                }
            }
            catch (AmazonSQSException e)
            {
                throw new Exception("Failed instantiating QueueAccess due to a failure in instantiating SQSClient", e);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed instantiating QueueAccess due to: {0}.", e.Message), e);
            }
        }
        #endregion Constructors

        #region Dispose
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            SqsConfig = null;
        //            AwsUser = null;
        //            SqsClient.Dispose();
        //        }
        //        this.disposed = true;
        //    }
        //}
        #endregion Dispose

        #region Public Methods
        public async Task<bool> FetchNext(bool logAndRequeueUnreadableMessages = true)
        {
            CurrentMessage = await GetNextMessage(logAndRequeueUnreadableMessages);
            return CurrentMessage != null;
        }
        public async Task<bool> FetchNextDeadMessage()
        {
            CurrentDeadLetter = await GetNextMessage(DeadLetterQueue);
            return CurrentDeadLetter != null;
        }
        public async Task<List<Message>> ProcessQueueMessages()
        {
            List<Message> list = null;
            try
            {
                list = new List<Message>();
                var receiveMessageResponse = await SqsClient.ReceiveMessageAsync(receiveMessageRequest);
                receiveMessageResponse.Messages.ForEach(m => { list.Add(m); });
            }
            catch (AmazonSQSException ex)
            {
                ErrorMessages.Add("QueueAccess.cs::ProcessQueueMessages()", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return list;
        }
        public async Task MoveCurrentMessageToDeadLetterQueue(bool deleteFromCurrentQueueAfterwards = false, string errorMessage = "")
        {
            await SendCurrentMessageToDeadLetterQueue(errorMessage);

            if (deleteFromCurrentQueueAfterwards)
            {
                await DeleteCurrentMessage(errorMessage);
            }
        }
        public async Task DeleteCurrentMessage(string errorMessage = "")
        {
            await DeleteMessageFromQueue(MessageQueue, CurrentMessage, errorMessage);
        }
        public async Task DeleteMessageFromQueue(ISqsQueueDto messageQueue, Message message, string errorMessage = "")
        {
            try
            {
                await SqsClient.DeleteMessageAsync(new DeleteMessageRequest(messageQueue.Url, message.ReceiptHandle));
            }
            catch (AmazonSQSException ex)
            {
                ErrorMessages.Add("QueueAccess.cs::DeleteMessageFromQueue(SqsQueue messageQueue, Message message)", 
                                                        SpliceMessage(ex.Message, FormatErrorMessage(errorMessage)), ex);
                if (!suppressExceptions) throw;
            }
        }
        public async Task<bool> SendMessage(ISqsQueueDto queue, Message message, string messageGroupId, string errorMessage = "")
        {
            return await SendMessage(message, queue.Url, messageGroupId, errorMessage);
        }
        public async Task<bool> SendCurrentMessage()
        {
            return await SendMessage(CurrentMessage, MessageQueue.Url, MessageGroupId);
        }
        public async Task<bool> SendMessage(Message message, string messageGroupId)
        {
            return await SendMessage(message, MessageQueue.Url, messageGroupId);
        }
        public async Task<bool> SendMessage(Message message, string queueUrl, string messageGroupId, string errorMessage = "")
        {
            bool sendResult = false;
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = SpliceMessage(message.Body, errorMessage),
                MessageGroupId = messageGroupId
            };

            try
            {
                var queueResult = await SqsClient.SendMessageAsync(sendMessageRequest);
                sendResult = !string.IsNullOrEmpty(queueResult.MessageId);
            }
            catch (AmazonSQSException ex)
            {
                sendResult = false;
                ErrorMessages.Add("QueueAccess.cs::SendMessage(Message message, string queueUrl, string messageGroupId)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return sendResult;
        }
        public async Task<Message> GetNextMessage(bool logAndRequeueUnreadableMessages = true)
        {
            return await GetNextMessage(MessageQueue, logAndRequeueUnreadableMessages);
        }
        public async Task<Message> GetNextMessage(ISqsQueueDto messageQueue, bool logAndRequeueUnreadableMessages = true)
        {
            Message message = null;
            var receiveMessageRequest = new ReceiveMessageRequest()
            {
                QueueUrl = messageQueue.Url
                ,
                MaxNumberOfMessages = 1
                ,
                WaitTimeSeconds = WaitTimeSeconds
                ,
                VisibilityTimeout = VisibilityTimeout
                ,
                AttributeNames = new List<string>() { "All" }
                ,
                MessageAttributeNames = new List<string>() { "All" }
            };
            try
            {
                var receiveMessageResponse = await SqsClient.ReceiveMessageAsync(receiveMessageRequest);
                message = receiveMessageResponse.Messages.Count > 0 ? receiveMessageResponse.Messages?[0] : null;
                if (message != null)
                {
                    FetchedMessages.Add(message);
                }
            }
            catch (AmazonSQSException ex)
            {
                ErrorMessages.Add("QueueAccess.cs::GetNextMessage(SqsQueue messageQueue)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return message;
        }
        public async Task<bool> IsReadableMessage()
        {
            bool isReadable = false;
            switch (CurrentMessage)
            {
                case null:
                    await DeleteCurrentMessage("Message is null");
                    break;
                default:
                    {
                        try
                        {
                            var messageBody = MessageDto.Parse(CurrentMessage)?.Body?.Trim();
                            isReadable = true;
                        }
                        catch (TypeInitializationException ex)
                        {
                            await MoveCurrentMessageToDeadLetterQueue(true,
                                        String.Format("Message content couldn't be properly parsed.  Exception content: {0}",
                                                        ex.Message));
                            if (!suppressExceptions) throw;
                        }
                        break;
                    }
            }
            return isReadable;
        }
        #endregion Public Methods

        #region Private Methods
        private AmazonSQSClient InstantiateSqsClient(IAwsUser awsUser, AmazonSQSConfig sqsConfig, bool suppressExceptions = true)
        {
            try
            {
                this._sqsClient = new AmazonSQSClient(awsUser.AccessKey, awsUser.SecretKey, sqsConfig.RegionEndpoint);
                //throw new AmazonSQSException("well well well");
            }
            catch (AmazonSQSException ex)
            {
                ErrorMessages.Add("QueueAccess.cs::InstantiateSqsClient(IAwsUser awsUser, AmazonSQSConfig sqsConfig)", ex.Message, ex);
                if (!suppressExceptions) throw;
            }
            return this._sqsClient;
        }
        private async Task SendCurrentMessageToDeadLetterQueue(string errorMessage = "")
        {
            DeadLetterMessages.Add(CurrentMessage);
            await SendMessage(DeadLetterQueue, CurrentMessage, DeadMessageGroupId, errorMessage);
        }
        private static string SpliceMessage(string messageBody, string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = FormatErrorMessage(errorMessage);
            }
            return string.Format("{0}{1}", errorMessage, messageBody);
        }
        private static string FormatErrorMessage(string errorMessage)
        {
            return string.Format("ERROR: {0}\nActual Body of Message Begins Below...\n", errorMessage);
        }
        #endregion Private methods
    }
}
