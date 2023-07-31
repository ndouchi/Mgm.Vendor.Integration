using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgm.VI.Aws.Sqs.Models
{
    public class MessageModel
    {
        public MessageModel() { }
        public MessageModel(string message, string url, string messageHandler, string messageGroupId = "")
        {
            this.Message = message;
            this.URL = url;
            this.MessageHandler = messageHandler;
            this.MessageGroupId = messageGroupId;
        }
        public string Message { get; set; }
        public string MessageGroupId { get; set; }

        public string URL { get; set; }
        public string MessageHandler { get; set; }



    }
}
