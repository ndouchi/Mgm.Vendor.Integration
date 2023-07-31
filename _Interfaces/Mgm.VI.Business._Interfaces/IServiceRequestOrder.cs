using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgm.VI.Business
{
    public interface IServiceRequestOrder : IServiceRequest
    {
        #region properties
        List<Message> FetchedMessages { get; }
        List<Message> DeadLetterMessages { get; }
        List<Message> ProcessedMessages { get; }
        IErrorMessages ErrorMessages { get; }
        IErrorMessages QueueAccessErrorMessages { get; }
        int FetchedMessagesCount { get; }
        int ProcessedMessagesCount { get; }
        int DeadLetterMessagesCount { get; }
        #endregion properties
        /*async*/
        Task ProcessVendorUpdates();
        /*async*/
        Task ProcessVendorUpdates(  ISqsQueueDto messageQueue,
                                    ISqsQueueDto deadLetterQueue,
                                    IAwsUser awsUser,
                                    AmazonSQSConfig sqsConfig,
                                    bool queueWithMessageGroupId,
                                    string messageGroupId,
                                    IRulesInspector rulesInspector
                                 );
    }
}
