using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgm.VI.Data.Dto;
using Mgm.VI.Logger;
using Mgm.VI.Aws;

namespace Mgm.VI.Business
{
    public interface IVendorStatusUpdate
    {
        #region properties
        IVendorDto Vendor { get; set; }
        IRulesInspector RulesInspector { get; set; }
        string XsdFilePath { get; set; }
        List<Message> FetchedMessages { get; }
        List<Message> DeadLetterMessages { get; }
        List<Message> ProcessedMessages { get; }
        List<IStatusUpdateDto> ProcessedStatusUpdateDtos { get; }
        List<IStatusUpdateDto> DeadStatusUpdateDtos { get; }
        IErrorMessages ErrorMessages { get; }
        IErrorMessages QueueAccessErrorMessages { get; }
        int FetchedMessagesCount { get; }
        int ProcessedMessagesCount { get; }
        int DeadLetterMessagesCount { get; }
        #endregion properties

        /*async*/ Task ProcessVendorUpdates();
        /*async*/ Task ProcessVendorUpdates(ISqsQueueDto messageQueue,
                                                ISqsQueueDto deadLetterQueue,
                                                IAwsUser awsUser,
                                                AmazonSQSConfig sqsConfig,
                                                bool queueWithMessageGroupId,
                                                string messageGroupId,
                                                ILoggerService logger,
                                                IRulesInspector rulesInspector
                                           );
    }
}
