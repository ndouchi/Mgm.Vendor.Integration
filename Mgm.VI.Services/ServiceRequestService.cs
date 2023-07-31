using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using Mgm.VI.Services;
using Mgm.VI.Common;

namespace Mgm.VI.Services
{
    public class ServiceRequestService : ServiceBase, IServiceRequestService
    {
        private bool disposed = false;
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
                this.disposed = true;
            }
        }
        #endregion Dispose

        private ContentFormat ServiceRequestContentFormat = ContentFormat.Unknown;
        public ServiceRequestSubmitResult SubmissionResult { get; private set; }
        public XDocument ServiceRequestXml { get; private set; }
        public string ServiceRequestJson { get; private set; }
        public Task VendorSubmit { get; private set; }

        public ServiceRequestService(string vendorUserId = "", string vendorId = "") : base(vendorUserId, vendorId) { }
        public ServiceRequestService(string serviceRequestJson, string vendorUserId = "", string vendorId = "") : this(vendorUserId, vendorId)
        {
            ServiceRequestContentFormat = ContentFormat.Json;
            ServiceRequestJson = serviceRequestJson;
        }
        public ServiceRequestService(XDocument serviceRequestXml, string vendorUserId = "", string vendorId = "") : this(vendorUserId, vendorId)
        {
            ServiceRequestContentFormat = ContentFormat.Xml;
            ServiceRequestXml = serviceRequestXml;
        }
        public async Task SubmitToVendor()
        {
            await ProcessSubmissionToVendor();
        }
        public async Task SubmitToVendor(XDocument serviceRequestXml, string vendorUserId, string vendorId)
        {
            __vendorUserId = vendorUserId;
            __vendorId = vendorId;
            ServiceRequestContentFormat = ContentFormat.Xml;
            ServiceRequestXml = serviceRequestXml;
            await ProcessSubmissionToVendorXml();
        }
        public async Task SubmitToVendor(string serviceRequestJson, string vendorUserId, string vendorId)
        {
            __vendorUserId = vendorUserId;
            __vendorId = vendorId;
            ServiceRequestContentFormat = ContentFormat.Json;
            ServiceRequestJson = serviceRequestJson;
            await ProcessSubmissionToVendorJson();
        }

        public IServiceRequestService SubmitServiceRequestToVendor()
        {
            VendorSubmit = SubmitToVendor();
            return this;
        }
        public IServiceRequestService SubmitServiceRequestToVendor(string serviceRequestJson, string vendorUserId, string vendorId)
        {
            VendorSubmit = SubmitToVendor(serviceRequestJson, vendorUserId, vendorId);
            return this;
        }
        public IServiceRequestService SubmitServiceRequestToVendor(XDocument serviceRequestXml, string vendorUserId, string vendorId)
        {
            VendorSubmit = SubmitToVendor(serviceRequestXml, vendorUserId, vendorId);
            return this;
        }
        /// <summary>
        /// PauseTilCompleted will only retrieve the last completed function
        /// </summary>
        /// <param name="threadSleepDuration"></param>
        public void PauseTilCompleted(int threadSleepDuration = 500)
        {
            while (!VendorSubmit.IsCompleted && !VendorSubmit.IsCanceled && !VendorSubmit.IsFaulted) System.Threading.Thread.Sleep(threadSleepDuration);
            SubmissionResult = new ServiceRequestSubmitResult("HttpResponse.StatusCode", HttpResponse.ReasonPhrase);//Needs Work
        }
        public void PauseTilCompleted(Task vendorSubmit, int threadSleepDuration = 500)
        {
            VendorSubmit = vendorSubmit;
            PauseTilCompleted(threadSleepDuration);
        }
        private async Task ProcessSubmissionToVendor()
        {
            switch (ServiceRequestContentFormat)
            {
                case ContentFormat.Json:
                    await ProcessSubmissionToVendorJson();
                    break;
                case ContentFormat.Xml:
                    await ProcessSubmissionToVendorXml();
                    break;
                default:
                    await ProcessDefault();
                    break;
            }
            //SubmissionResult = new ServiceRequestSubmitResult("vubiquity", "error");
        }
        private Task ProcessDefault()
        {
            throw new FormatException("ServiceRequestService Error: Invalid Content Format");
        }
        private Task ProcessSubmissionToVendorXml()
        {
            //Process serviceRequestXml
            throw new NotImplementedException();
        }
        private Task ProcessSubmissionToVendorJson()
        {
            //Process serviceRequestJson
            throw new NotImplementedException();
        }
    }
}
