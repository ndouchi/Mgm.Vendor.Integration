using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Rules;

namespace Mgm.VI.Aws.Sqs.Helpers
{
    [Obsolete("This functionality has been migrated to QueueAccess")]
    public class SQSHelper : IDisposable
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private AmazonSQSClient _client;
        private readonly RegionEndpoint _regionEndpoint;
        public SQSHelper(string accessKey, string secretkey, RegionEndpoint regionEndpoint)
        {
            this._accessKey = accessKey;
            this._secretKey = secretkey;
            this._regionEndpoint = regionEndpoint;
        }

        private AmazonSQSClient Client
        {
            get
            {
                if (_client == null)
                    _client = new AmazonSQSClient(this._accessKey, this._secretKey, this._regionEndpoint);
                return _client;
            }
        }


        public async Task CreateQueue(string queueName)
        {
            try
            {
                CreateQueueRequest createQueueRequest = new CreateQueueRequest();
                createQueueRequest.QueueName = queueName;
                CreateQueueResponse createQueueResponse = await Client.CreateQueueAsync(createQueueRequest);
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteQueue(string queueName)
        {
            try
            {
                DeleteQueueRequest deleteQueue = new DeleteQueueRequest();
                deleteQueue.QueueUrl = queueName;
                DeleteQueueResponse deleteQueueResponse = await Client.DeleteQueueAsync(deleteQueue);
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ListQueuesResponse> GetQueue()
        {
            ListQueuesRequest listQueues = new ListQueuesRequest();
            return await Client.ListQueuesAsync(listQueues);
        }

        public async Task SendMessage(string url, string message, string MessageGroupId = "")
        {
            try
            {
                SendMessageRequest sendMessageRequest = new SendMessageRequest();
                sendMessageRequest.QueueUrl = url;
                sendMessageRequest.MessageBody = message;
                await Client.SendMessageAsync(sendMessageRequest);
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteMessageAsync(string url, string handler)
        {
            try
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
                deleteMessageRequest.QueueUrl = url;
                deleteMessageRequest.ReceiptHandle = handler;
                await Client.DeleteMessageAsync(deleteMessageRequest);
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Message>> GetAllMessages(string url)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = url;
            ReceiveMessageResponse receiveMessageResponse = await Client.ReceiveMessageAsync(receiveMessageRequest);
            var list = new List<Message>();
            foreach (var msg in receiveMessageResponse.Messages)
            {
                list.Add(msg);
            }
            return list;
        }
        public async Task<Message> GetNextMessage(string url)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = url;
            ReceiveMessageResponse receiveMessageResponse = await Client.ReceiveMessageAsync(receiveMessageRequest);
            return receiveMessageResponse.Messages?[0];
        }
        public async Task<Message> GetNextMessage(SqsQueueDto messageQueue, 
                                                    SqsQueueDto deadLetterQueue, 
                                                    AwsUser awsUser,
                                                    AmazonSQSConfig sqsConfig, 
                                                    IRulesInspector rulesInspector)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest()
            {
                QueueUrl = messageQueue.Url
                ,MaxNumberOfMessages = 1
                ,AttributeNames = new List<string>() { "All" }
                ,MessageAttributeNames = new List<string>() { "All" }
            };
            ReceiveMessageResponse receiveMessageResponse = await Client.ReceiveMessageAsync(receiveMessageRequest);
            return receiveMessageResponse.Messages?[0];
        }
        public async Task<Message> GetNextMessage(SqsQueueDto messageQueue, 
                                                    SqsQueueDto deadLetterQueue, 
                                                    AwsUser awsUser, 
                                                    AmazonSQSConfig sqsConfig,
                                                    IRulesInspector rulesInspector, 
                                                    int maxRetries = 20)
        {
            var message = new Message();
            int numOfRetries = 0;
            using (AmazonSQSClient sqsClient = new AmazonSQSClient(sqsConfig))
                while (numOfRetries++ < maxRetries)
                {
                    ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest()
                    {
                        QueueUrl = messageQueue.Url,
                        MaxNumberOfMessages = 1,
                        AttributeNames = new List<string>() { "All" },
                        MessageAttributeNames = new List<string>() { "All" }
                    };

                    var receiveMessageResponse = sqsClient.ReceiveMessageAsync(receiveMessageRequest).Result;
                    
                    if (receiveMessageResponse.Messages.Count > 0)
                    {
                        message = receiveMessageResponse.Messages?[0];
                        numOfRetries = maxRetries;
                    }
                }
            return message;
        }
        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
