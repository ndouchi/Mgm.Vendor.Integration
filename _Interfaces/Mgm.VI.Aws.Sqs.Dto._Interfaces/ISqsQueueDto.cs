using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Mgm.VI.Aws.Sqs.Dto 
{
    public interface ISqsQueueDto
    {
        #region Properties
        ISqsQueueDto DeadLetterQueue { get; set; }
        bool QueueIsDefined { get; }
        bool IsDeadLetterQueue { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string Arn { get; set; }
        DateTime Created { get; set; }
        DateTime LastUpdated { get; set; }
        /// <summary>
        /// DeliveryDelay in seconds
        /// </summary>
        int DeliveryDelay { get; set; }

        QType QueueType { get; set; }
        /// <summary>
        /// ContentBasedDeduplication [Enabled = 1, Disabled = 0]
        /// </summary>
        bool ContentBasedDeduplication { get; set; }
        RedrivePolicyStruct RedrivePolicy { get; set; }

        /// <summary>
        /// DefaultVisibilityTimeout in seconds
        /// </summary>
        // Restrict value to less than a 1000
        int DefaultVisibilityTimeout { get; set; }

        /// <summary>
        /// MessageRetentionPeriod in days, maximum is 14
        /// </summary>
        short MessageRetentionPeriod { get; set; }
        /// <summary>
        /// MaximumMessageSize less than or equals to 256 (KB)
        /// </summary>
        short MaximumMessageSize { get; set; }

        short ReceiveMessageWaitTime { get; set; }
        short MessagesAvailableVisible { get; set; }
        short MessagesInFlightNotVisible { get; set; }
        short MessagesDelayed { get; set; }


        #endregion Properties
    }
}

