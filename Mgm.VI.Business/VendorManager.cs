using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mgm.VI.Aws.Sqs;
using Mgm.VI.Aws.Sqs.Dto;
using Mgm.VI.Aws.Sqs.Helpers;
using Mgm.VI.Repository;
using Mgm.VI.Aws.Sqs.Rules;
using Mgm.VI.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgm.VI.Data.Dto;
using Mgm.VI.Business;

namespace Mgm.VI.Business
{
    public class VendorManager : IVendorManager
    {
        private Storage storage { get; set; }
        #region properties
        public IErrorMessages ErrorMessages { get; private set; }
        #endregion properties

        public VendorManager(Storage storage, IErrorMessages errorMessages = null)
        {
            Initialize(storage, errorMessages);
        }
        private void Initialize(Storage storage, IErrorMessages errorMessages = null)
        {
            this.storage = storage;
            ErrorMessages = errorMessages ?? new ErrorMessages();
        }
        private void LogException(string errorSource, string errorText, Exception e)
        {
            if (ErrorMessages == null) ErrorMessages = new ErrorMessages();
            ErrorMessages.Add(errorSource, errorText, e);
        }
        public int AddVendor(IVendorDto vendorDto)
        {
            if (string.IsNullOrEmpty(vendorDto.VendorId.Trim())) return -1;
            using (VendorUnitOfWork vuow = new VendorUnitOfWork(storage))
            {
                return vuow.AddVendor(vendorDto);
            }
        }
        public int DeleteVendor(string vendorId)
        {
            if (string.IsNullOrEmpty(vendorId.Trim())) return -1;
            using (VendorUnitOfWork vuow = new VendorUnitOfWork(storage))
            {
                return vuow.DeleteVendor(vendorId);
            }
        }

        public IVendorDto GetVendor(string vendorId)
        {
            if (string.IsNullOrEmpty(vendorId)) return null;
            using (VendorUnitOfWork vuow = new VendorUnitOfWork(storage))
            {
                var vendor = vuow.GetVendor(vendorId);
                return vendor;
            }
        }
        public List<IVendorDto> GetVendors()
        {
            using (VendorUnitOfWork vuow = new VendorUnitOfWork(storage))
            {
                var vendors = vuow.GetAllVendors();
                return vendors;
            }
        }
        public int UpdateVendor(IVendorDto vendorDto)
        {
            if (vendorDto == null || string.IsNullOrEmpty(vendorDto.VendorId.Trim())) return -1;
            using (VendorUnitOfWork vuow = new VendorUnitOfWork(storage))
            {
                return vuow.UpdateVendor(vendorDto);
            }
        }
    }
}
