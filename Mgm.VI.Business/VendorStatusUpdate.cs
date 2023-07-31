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
using Mgm.VI.Business;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws;
using Mgm.VI.Aws.Sqs;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Repository;

namespace Mgm.VI.Business
{
    public class VendorStatusUpdate : IVendorStatusUpdate
    {
        #region imported from Processor
        //  private static Microsoft.Extensions.Configuration.IConfiguration Configuration;
        private IAwsUser _awsUser;
        private Amazon.SQS.AmazonSQSConfig _sqsConfig;
        private string _vendorId = "vubiquityId";// To be read from DB or commandline argumentsN
        private ISqsQueueDto _messageQueue, _deadLetterQueue;
        private string _messageGroupId = Guid.NewGuid().ToString();
        private bool _queueWithMessageGroupId = false;
        private IRulesInspector _rulesInspector;
        private Storage storage { get; set; }
        private  ILoggerService Logger;

        #endregion  imported from Processor

        #region properties
        public IRulesInspector RulesInspector
        {
            get
            {
                return _rulesInspector ?? new RulesInspector();
            }
            set
            {
                _rulesInspector = value;
            }
        }
        public string XsdFilePath { get; set; }
        public IVendorDto Vendor { get; set; }
        public List<Message> FetchedMessages { get; private set; }
        public List<Message> DeadLetterMessages { get; private set; }
        public List<Message> ProcessedMessages { get; private set; }
        public List<IStatusUpdateDto> ProcessedStatusUpdateDtos { get; private set; }
        public List<IStatusUpdateDto> DeadStatusUpdateDtos { get; private set; }
        public IErrorMessages ErrorMessages { get; private set; }
        public IErrorMessages QueueAccessErrorMessages { get; private set; }
        public int FetchedMessagesCount { get; private set; }
        public int ProcessedMessagesCount { get; private set; }
        public int DeadLetterMessagesCount { get; private set; }

        public IQueueAccess QueueAccess { get; set; }
        #endregion properties

        public VendorStatusUpdate(
                                    Storage storage,
                                    IVendorDto vendor,
                                    string xsdFilePath,
                                    SqsQueueDto messageQueue,
                                    SqsQueueDto deadLetterQueue,
                                    AwsUser awsUser,
                                    AmazonSQSConfig sqsConfig,
                                    bool queueWithMessageGroupId,
                                    string messageGroupId,
                                    ILoggerService logger,
                                    IRulesInspector rulesInspector)
        {
            Initialize(storage, vendor, xsdFilePath, messageQueue, deadLetterQueue, awsUser, sqsConfig, 
                        queueWithMessageGroupId, messageGroupId, logger, rulesInspector);
        }

        public VendorStatusUpdate(Storage storage, IVendorDto vendor, string xsdFilePath, ILoggerService logger, IQueueAccess qa)
        {
            Initialize(storage, vendor, xsdFilePath, logger, qa);
        }
        private void Initialize(Storage storage, 
                                IVendorDto vendor, 
                                string xsdFilePath, 
                                SqsQueueDto messageQueue, 
                                SqsQueueDto deadLetterQueue, 
                                AwsUser awsUser, 
                                AmazonSQSConfig sqsConfig, 
                                bool queueWithMessageGroupId, 
                                string messageGroupId,
                                ILoggerService logger,
                                IRulesInspector rulesInspector)
        {
        IQueueAccess qa = new QueueAccess(  awsUser,
                                                sqsConfig,
                                                messageQueue,
                                                deadLetterQueue,
                                                queueWithMessageGroupId,
                                                messageGroupId,
                                                logger,
                                                rulesInspector);
            Initialize(storage, vendor, xsdFilePath, logger, qa);
        }
        private void Initialize(Storage storage, IVendorDto vendor, 
                                string xsdFilePath, ILoggerService logger, IQueueAccess qa)
        {
            this.storage = storage;
            this.QueueAccess = qa;
            this.Vendor = vendor;
            this.XsdFilePath = xsdFilePath;
            this._messageQueue = qa.MessageQueue;
            this._deadLetterQueue = qa.DeadLetterQueue;
            this._awsUser = qa.AwsUser;
            this._sqsConfig = qa.SqsConfig;
            this._queueWithMessageGroupId = qa.QueueWithMessageGroupId;
            this._messageGroupId = qa.MessageGroupId;
            this._rulesInspector = qa.RulesInspector;
            this.Logger = logger;

            FetchedMessages = new List<Message>();
            DeadLetterMessages = new List<Message>();
            ProcessedMessages = new List<Message>();
        }

        public async Task ProcessVendorUpdates()
        {
            await ProcessVendorUpdates(this.QueueAccess);
        }
        public async Task ProcessVendorUpdates( ISqsQueueDto messageQueue,
                                                ISqsQueueDto deadLetterQueue,
                                                IAwsUser awsUser,
                                                AmazonSQSConfig sqsConfig,
                                                bool queueWithMessageGroupId,
                                                string messageGroupId,
                                                ILoggerService logger,
                                                IRulesInspector rulesInspector
                                                )
        {
            QueueAccess = new QueueAccess(awsUser,
                                                    sqsConfig,
                                                    messageQueue,
                                                    deadLetterQueue,
                                                    queueWithMessageGroupId,
                                                    messageGroupId,
                                                    logger,
                                                    rulesInspector);
            await ProcessVendorUpdates(QueueAccess);
        }
        private async Task ProcessVendorUpdates(IQueueAccess qa)
        {
            try
            {
                string messageBody = null;
                while (await qa.FetchNext())
                {
                    /// TODO: Move this switch statement to QueueAccess and turn it into InspectMessage and either return a pointer
                    /// to the QueueAccess object after it has refetched the next message or???
                    switch (qa.CurrentMessage)
                    {
                        case null:
                            await qa.DeleteCurrentMessage("Message is null");
                            continue;
                        default:
                            {
                                try
                                {
                                    messageBody = MessageDto.Parse(qa.CurrentMessage)?.Body?.Trim();
                                }
                                catch (TypeInitializationException ex)
                                {
                                    await qa.MoveCurrentMessageToDeadLetterQueue(true,
                                                String.Format("Message content couldn't be properly parsed.  Exception content: {0}",
                                                                ex.Message));
                                    continue;
                                }

                                break;
                            }
                    }
                    RulesInspector = qa.RulesInspector.SetContent(messageBody).Validate();

                    using (StatusUpdateUnitOfWork suUow =
                        new StatusUpdateUnitOfWork(qa.RulesInspector.RulesAreMet, messageBody, storage))
                    {
                        try
                        {
                            if (suUow.Save())
                                ProcessedMessages.Add(qa.CurrentMessage);
                            else
                            {
                                if (!Logger.Log(qa.CurrentMessage.Body))
                                {
                                    try
                                    {
                                        Logger.ChangeToApplicationLog().Log((qa.CurrentMessage.Body));
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }

                            if (!suUow.AllProcessed)
                                await qa.MoveCurrentMessageToDeadLetterQueue(true);
                        }
                        #region Exceptions
                        catch (AggregateException e)
                        {
                            LogException("VendorStatusUpdate::ProcessVendorUpdates::using(UnitOfWork...)", e.Message, e);
                        }
                        catch (Exception e)
                        {
                            LogException("VendorStatusUpdate::ProcessVendorUpdates::using(UnitOfWork...)", e.Message, e);
                        }
                        #endregion Exceptions

                        ProcessedStatusUpdateDtos.AddRange(suUow.ProcessedStatusUpdateDtos);
                        DeadStatusUpdateDtos.AddRange(suUow.DeadStatusUpdateDtos);
                    }
                }
                FetchedMessages = new List<Message>(qa.FetchedMessages.ToArray());
                DeadLetterMessages = new List<Message>(qa.DeadLetterMessages.ToArray());
                QueueAccessErrorMessages = new ErrorMessages(new List<IErrorMessage>(qa.ErrorMessages));
            }
            catch (AggregateException e)
            {
                LogException("VendorStatusUpdate::ProcessVendorUpdates", e.Message, e);
            }
            catch (Exception e)
            {
                LogException("VendorStatusUpdate::ProcessVendorUpdates", e.Message, e);
            }
        }

        private void ReportMessage(ServiceRequestStatusRule srRule, IQueueAccess qa)
        {
            throw new NotImplementedException();
        }

        private List<IStatusUpdateDto> ParseStatusUpdateDtos(string messageBody)
        {
            if (messageBody == null) return null;
            try
            {
                var sqsRetrievalTimestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                var comments = string.Empty;
                var isPersistedToMss = false;
                var suDtos = StatusUpdateDto.Parse(XmlProcessor.LoadContent(messageBody),
                                                            XsdFilePath,
                                                            Constants.MssXsd.ServiceRequest,
                                                            Vendor, 
                                                            comments,
                                                            messageBody,
                                                            sqsRetrievalTimestamp,
                                                            isPersistedToMss
                                                            );
                return suDtos;
            }
            catch (Exception e)
            {
                LogException("VendorStatusUpdate::ParseDtos(...)", e.Message, e);
                return null;
            }
        }
        private void LogException(string errorSource, string errorText, Exception e)
        {
            if (QueueAccessErrorMessages == null) QueueAccessErrorMessages = new ErrorMessages();
            QueueAccessErrorMessages.Add(errorSource, errorText, e);
        }
    }
}
