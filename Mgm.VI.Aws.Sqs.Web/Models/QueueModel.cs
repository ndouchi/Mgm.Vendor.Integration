using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mgm.Aws.Sqs.Models
{
    public class QueueModel
    {
        public QueueModel() { }
        public QueueModel(string queueName)
        {
            this.QueueName = queueName;
        }
        [Display(Name ="Queue Name")]
        public string QueueName { get; set; }
    }
}
