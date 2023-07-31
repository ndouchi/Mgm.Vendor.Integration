using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mgm.Aws.Sqs.Dto
{
    public class VendorDto
    {
        #region Properties
        [Display(Name = "Vendor Id")]
        public string VendorId { get; set; }
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }
        [Display(Name = "Queue Name")]
        public string QueueName { get; set; }
        #endregion Properties



        public VendorDto() { }
        //public Vendor(string vendorId, Queu)
        //{
        //    this.VendorId = vendorId;
        //    this.QueueName = queueName;
        //}
    }
}
