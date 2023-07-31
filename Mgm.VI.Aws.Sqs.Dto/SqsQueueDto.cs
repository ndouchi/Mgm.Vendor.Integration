using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Data.Dto;

namespace Mgm.VI.Aws.Sqs.Dto 
{
    public class SqsQueueDto : DtoBase, ISqsQueueDto
    {
        #region Properties
        public ISqsQueueDto DeadLetterQueue { get; set; }
        public bool QueueIsDefined { get; }
        public bool IsDeadLetterQueue { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Arn { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// DeliveryDelay in seconds
        /// </summary>
        public int DeliveryDelay { get; set; }

        public QType QueueType { get; set; }
        /// <summary>
        /// ContentBasedDeduplication [Enabled = 1, Disabled = 0]
        /// </summary>
        public bool ContentBasedDeduplication { get; set; }
        public RedrivePolicyStruct RedrivePolicy { get; set; }

        /// <summary>
        /// DefaultVisibilityTimeout in seconds
        /// </summary>
        // Restrict value to less than a 1000
        public int DefaultVisibilityTimeout { get; set; }

        /// <summary>
        /// MessageRetentionPeriod in days, maximum is 14
        /// </summary>
        public short MessageRetentionPeriod { get; set; }
        /// <summary>
        /// MaximumMessageSize less than or equals to 256 (KB)
        /// </summary>
        public short MaximumMessageSize { get; set; }

        public short ReceiveMessageWaitTime { get; set; }
        public short MessagesAvailableVisible { get; set; }
        public short MessagesInFlightNotVisible { get; set; }
        public short MessagesDelayed { get; set; }


        #endregion Properties
        public SqsQueueDto() { }
        public SqsQueueDto(Amazon.SQS.Model.ReceiveMessageResponse queue)
        {
        }
        public SqsQueueDto(string queueName, string queueUrl, QType qType, ISqsQueueDto deadLetterQueue = null)
        {
            this.Name = queueName;
            this.Url = queueUrl;
            this.QueueType = qType;
            this.DeadLetterQueue = deadLetterQueue;
            this.QueueIsDefined = true;
        }
    }
}

