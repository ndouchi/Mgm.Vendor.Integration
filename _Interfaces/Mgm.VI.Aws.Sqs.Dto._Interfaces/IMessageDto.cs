using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SQS.Model;

namespace Mgm.VI.Aws.Sqs.Dto
{
    public interface IMessageDto
    {
        #region Properties
        Dictionary<string, string> Attributes { get; set; }
        Dictionary<string, MessageAttributeValue> MessageAttributes { get; set; }
        string Body { get; set; }
        string MessageId { get; set; }
        string ReceiptHandle { get; set; }
        string MessageGroupId { get; set; }
        string MD5OfBody { get; set; }
        string MD5OfMessageAttributes { get; set; }
        #endregion Properties
        XElement ToXElement();
        //static IMessageDto Parse(Message message);
//        static XElement ParseXElement(Message currentMessage);
    }
}
