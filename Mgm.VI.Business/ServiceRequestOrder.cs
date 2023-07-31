using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.Aws.Sqs;
using Mgm.Aws.Sqs.Dto;
using Mgm.Aws.Sqs.Helpers;
using Mgm.Mss.Sqs.Repository;
using Mgm.Aws.Sqs.Rules;
using Mgm.Mss.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgm.Mss.Data.Dto;

namespace Mgm.Aws.Business
{
    public class ServiceRequestOrder : ServiceRequest_Base, IServiceRequest
    {
        #region imported from Processor
        //  private static Microsoft.Extensions.Configuration.IConfiguration Configuration;
        //private string _defaultMessageGroupId = "123456"; //This can be a vendorId if we choose to go with one queue for all vendors
        //private string messageGroupId = "";
        private AwsUser _awsUser;
        private Amazon.SQS.AmazonSQSConfig _sqsConfig;
        private Storage storage;
        private string xmlRecordContent;
        private ServiceRequestDto srDto;
        private IRulesInspector RulesInspector { get; set; }
        private string _vendorId = "vubiquityId";// To be read from DB or commandline arguments
        private Sqs.Dto.SqsQueueDto _messageQueue, _deadLetterQueue;
        private string _messageGroupId = Guid.NewGuid().ToString();
        private bool _queueWithMessageGroupId = false;
        #endregion  imported from Processor

        #region properties
        public List<Message> FetchedMessages { get; private set; }
        public List<Message> DeadLetterMessages { get; private set; }
        public List<Message> ProcessedMessages { get; private set; }
        public IErrorMessages ErrorMessages { get; private set; }
        public IErrorMessages QueueAccessErrorMessages { get; private set; }
        public int FetchedMessagesCount { get; private set; }
        public int ProcessedMessagesCount { get; private set; }
        public int DeadLetterMessagesCount { get; private set; }
        #endregion properties

        public ServiceRequestOrder(string vendorId,
                                    SqsQueueDto messageQueue,
                                    SqsQueueDto deadLetterQueue,
                                    AwsUser awsUser,
                                    AmazonSQSConfig sqsConfig,
                                    bool queueWithMessageGroupId,
                                    string messageGroupId,
                                    IRulesInspector rulesInspector)
        {
            this._vendorId = vendorId;
            this._messageQueue = messageQueue;
            this._deadLetterQueue = deadLetterQueue;
            this._awsUser = awsUser;
            this._sqsConfig = sqsConfig;
            this._queueWithMessageGroupId = queueWithMessageGroupId;
            this._messageGroupId = messageGroupId;
            RulesInspector = rulesInspector;
            FetchedMessages = new List<Message>();
            DeadLetterMessages = new List<Message>();
            ProcessedMessages = new List<Message>();
        }

        public async Task ProcessVendorUpdates()
        {
            await ProcessVendorUpdates(_messageQueue,
                                    _deadLetterQueue,
                                    _awsUser,
                                    _sqsConfig,
                                    _queueWithMessageGroupId,
                                    _messageGroupId,
                                    RulesInspector);
        }

        public async Task ProcessVendorUpdates( SqsQueueDto messageQueue,
                                                SqsQueueDto deadLetterQueue,
                                                AwsUser awsUser,
                                                AmazonSQSConfig sqsConfig,
                                                bool queueWithMessageGroupId,
                                                string messageGroupId,
                                                IRulesInspector rulesInspector
                                                )
        {
            IQueueAccess qa = new QueueAccess(awsUser,
                                                    sqsConfig,
                                                    messageQueue,
                                                    deadLetterQueue,
                                                    queueWithMessageGroupId,
                                                    messageGroupId,
                                                    rulesInspector);
                while (await qa.FetchNext())
                {
                    var messageBody = MessageDto.Parse(qa.CurrentMessage).Body.Trim();
                    RulesInspector = rulesInspector.Validate();
                    if (rulesInspector.RulesAreMet)
                        {
                            using (StatusUpdateUnitOfWork suUow = new StatusUpdateUnitOfWork(true, null, storage, null))
                            {
                                List<IServiceRequestDto> srDtos =
                                    ServiceRequestDto.Parse(XmlProcessor.LoadContent(messageBody),
                                                            XsdFilePath,
                                                            Constants.MssXsd.ServiceRequest);

                                if (srDtos != null && srDtos.Count > 0)
                                //    srDto = new ServiceRequestStatusUpdate(_vendorId, xmlRecordContent);
                                {
                                    //srDtos.ForEach(srDto => { repo.Add(srDto, qa.CurrentMessage.Body);  });
                                    srDtos.ForEach(srDto => { suUow.ServiceRequestDtos.Add(srDto); });
                                    if (suUow.Save())
                                        ProcessedMessages.Add(qa.CurrentMessage);
                                    else
                                    {
                                        await qa.MoveCurrentMessageToDeadLetterQueue(true);
                                    }
                                }
                                else // If CurrentMessage contains no valid srDtos
                                    await qa.MoveCurrentMessageToDeadLetterQueue(true);
                            }
                        }
                    else
                        {
                            using (ErrorLogSqlRepository repo = new ErrorLogSqlRepository(storage.ConnectionString))
                            {
                                //repo.Add(srDto);
                                repo.Save();
                            }
                            await qa.MoveCurrentMessageToDeadLetterQueue(true);
                        }
                }
            FetchedMessages = new List<Message>(qa.FetchedMessages.ToArray());
            DeadLetterMessages = new List<Message>(qa.DeadLetterMessages.ToArray());
            QueueAccessErrorMessages.CopyFrom(qa.ErrorMessages);
        }
    }
}
