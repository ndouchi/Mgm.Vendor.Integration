using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgm.VI.Aws.Sqs.Models
{
    public class ServiceRequestModel
    {
        public ServiceRequestModel() { }
        public ServiceRequestModel(string serviceRequest, string vendorId)
        {
            this.ServiceRequest = serviceRequest;
            this.VendorId = vendorId;
        }
        public string ServiceRequest { get; set; }
        public string VendorId { get; set; }
    }
}
