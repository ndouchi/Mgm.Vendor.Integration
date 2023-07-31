using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Common;

namespace Mgm.VI.Aws.Sqs.Dto
{
    public class MessageDto : IMessageDto
    {
        private const string InitializationError = "The injected SqsMessage cannot be null";
        #region Private Vars
        private Message _message;
        #endregion Private Vars
        public IErrorMessages ErrorMessages { get; set; }
        #region Constructors
        public MessageDto() { }
        public MessageDto(Message sqsMessage, IErrorMessages errorMessages = null)
        {
            _message = sqsMessage ?? throw new TypeInitializationException("Message", new Exception(InitializationError));
            ErrorMessages = errorMessages ?? new ErrorMessages();
            this.MessageId = sqsMessage.MessageId ?? "";
            this.Body = sqsMessage.Body ?? "";
            this.ReceiptHandle = sqsMessage.ReceiptHandle ?? "";

        }
        //public MessageDto(string messageContent, string url, string messageHandler, string messageGroupId = "")
        //{
        //    this.MessageContent = messageContent;
        //    this.URL = url;
        //    this.MessageHandler = messageHandler;
        //    this.MessageGroupId = messageGroupId;
        //}
        #endregion Constructors

        #region Properties
        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, MessageAttributeValue> MessageAttributes { get; set; }
        public string Body { get; set; }
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }
        public string MessageGroupId { get; set; }
        public string MD5OfBody { get; set; }
        public string MD5OfMessageAttributes { get; set; }
        #endregion Properties
        public static IMessageDto Parse(Message message)
        {
            if (message == null) return null; //throw new TypeInitializationException("Message", new Exception(InitializationError));
            var messageDto = new MessageDto(message);
            return messageDto;
        }
        public XElement ToXElement()
        {
            return ParseXElement(_message);
        }
        public static XElement ParseXElement(Message currentMessage)
        {
            var mdto = Parse(currentMessage);
            XElement xElement = XElement.Parse(mdto?.Body);
            return xElement;
        }
    }
}
