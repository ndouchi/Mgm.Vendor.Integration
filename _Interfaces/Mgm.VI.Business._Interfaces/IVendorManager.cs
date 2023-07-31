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

namespace Mgm.VI.Business
{
    public interface IVendorManager
    {
        #region properties
        IErrorMessages ErrorMessages { get; }
        #endregion properties

        int AddVendor(IVendorDto vendor);
        int DeleteVendor(string vendorId);
        IVendorDto GetVendor(string vendorId);
        List<IVendorDto> GetVendors();
        int UpdateVendor(IVendorDto vendor);
    }
}
