using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgm.VI.Aws.Sqs
{
    public interface IQueueAccess //: IDisposable
    {
        #region Properties
        bool SuppressExceptions {get; set;}
        AmazonSQSConfig SqsConfig { get; }
        IAwsUser AwsUser { get; }
        Message CurrentMessage { get; }
        Message CurrentDeadLetter { get; }
        List<Message> FetchedMessages { get; }
        List<Message> DeadLetterMessages { get; }
        IErrorMessages ErrorMessages { get; }
        ISqsQueueDto MessageQueue { get; }
        ISqsQueueDto DeadLetterQueue { get; }
        AmazonSQSClient SqsClient { get; }
        string MessageGroupId { get; set; }
        string DeadMessageGroupId { get; set; }
        bool QueueWithMessageGroupId { get; }
        IRulesInspector RulesInspector { set; get; }
        #endregion Properties

        #region Public Methods
        /*async*/
        Task<bool> FetchNext(bool logAndRequeueUnreadableMessages = true);

        /*async*/ Task<bool> FetchNextDeadMessage();
        /*async*/ Task<List<Message>> ProcessQueueMessages();
        /*async*/
        Task MoveCurrentMessageToDeadLetterQueue(bool deleteFromCurrentQueueAfterwards = false, string errorMessage = "");
        /*async*/
        Task DeleteCurrentMessage(string errorMessage = "");
        /*async*/
        Task DeleteMessageFromQueue(ISqsQueueDto messageQueue, Message message, string errorMessage = "");
        /*async*/
        Task<bool> SendMessage(ISqsQueueDto queue, Message message, string messageGroupId, string errorMessage = "");
        /*async*/
        Task<bool> SendCurrentMessage();
        /*async*/
        Task<bool> SendMessage(Message message, string messageGroupId);
        /*async*/
        Task<bool> SendMessage(Message message, string queueUrl, string messageGroupId, string errorMessage = "");
        /*async*/
        Task<Message> GetNextMessage(bool logAndRequeueUnreadableMessages = true);
        /*async*/
        Task<Message> GetNextMessage(ISqsQueueDto messageQueue, bool logAndRequeueUnreadableMessages = true);
        #endregion Public Methods
    }
}