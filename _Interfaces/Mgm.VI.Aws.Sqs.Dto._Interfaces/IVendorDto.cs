using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mgm.Aws.Sqs.Dto
{
    public interface IVendorDto
    {
        #region Properties
        string VendorId { get; set; }
        string VendorName { get; set; }
        string QueueName { get; set; }
        #endregion Properties
    }
}
