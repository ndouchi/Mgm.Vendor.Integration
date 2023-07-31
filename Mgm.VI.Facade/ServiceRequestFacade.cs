using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using Mgm.VI.Services;
using Mgm.VI.Repository;
using Mgm.VI.Common;

namespace Mgm.VI.Facade
{
    public class ServiceRequestFacade : FacadeBase, IServiceRequestFacade
    {
        private IServiceRequestService serviceRequestService;
        private ServiceRequestHistoryUnitOfWork srUow;
        private ContentFormat ServiceRequestContentFormat = ContentFormat.Unknown;

        public ServiceRequestSubmitResult SubmissionResult { get; private set; }
        public XDocument ServiceRequestXml { get; private set; }
        public string ServiceRequestJson { get; private set; }
        public Task VendorSubmit { get; private set; }
     
        public ServiceRequestFacade() : this(string.Empty, string.Empty) {}
        public ServiceRequestFacade(string vendorUserId, string vendorId) : base(vendorUserId, vendorId) {}
        public ServiceRequestFacade(string serviceRequestJson, string vendorUserId = "", string vendorId = "") : this(vendorUserId, vendorId)
        {
            ServiceRequestContentFormat = ContentFormat.Json;
            ServiceRequestJson = serviceRequestJson;
            serviceRequestService = new ServiceRequestService(ServiceRequestJson, vendorUserId, vendorId);
        }
        public ServiceRequestFacade(XDocument serviceRequestXml, string vendorUserId = "", string vendorId = "") : this(vendorUserId, vendorId)
        {
            ServiceRequestContentFormat = ContentFormat.Xml;
            ServiceRequestXml = serviceRequestXml;
            serviceRequestService = new ServiceRequestService(ServiceRequestXml, vendorUserId, vendorId);
        }
        public async Task SubmitToVendor()
        {
            await ProcessSubmissionToVendor();
        }
        public async Task SubmitToVendor(string serviceRequestJson, string vendorUserId, string vendorId)
        {
            __vendorUserId = vendorUserId;
            __vendorId = vendorId;
            ServiceRequestContentFormat = ContentFormat.Json;
            ServiceRequestJson = serviceRequestJson;
            await ProcessSubmissionToVendorJson();
        }
        public async Task SubmitToVendor(XDocument serviceRequestXml, string vendorUserId, string vendorId)
        {
            __vendorUserId = vendorUserId;
            __vendorId = vendorId;
            ServiceRequestContentFormat = ContentFormat.Xml;
            ServiceRequestXml = serviceRequestXml;
            await ProcessSubmissionToVendorXml();
        }

        public IServiceRequestFacade SubmitServiceRequestToVendor()
        {
            VendorSubmit = SubmitToVendor();
            return this;
        }
        public IServiceRequestFacade SubmitServiceRequestToVendor(string serviceRequestJson, string vendorUserId, string vendorId)
        {
            VendorSubmit = SubmitToVendor(serviceRequestJson, vendorUserId, vendorId);
            return this;
        }
        public IServiceRequestFacade SubmitServiceRequestToVendor(XDocument serviceRequestXml, string vendorUserId, string vendorId)
        {
            VendorSubmit = SubmitToVendor(serviceRequestXml, vendorUserId, vendorId);
            return this;
        }
 
        public void PauseTilCompleted(int threadSleepDuration = 500)
        {
            //while (!VendorSubmit.IsCompleted && !VendorSubmit.IsCanceled && !VendorSubmit.IsFaulted) System.Threading.Thread.Sleep(threadSleepDuration);
            serviceRequestService.PauseTilCompleted(threadSleepDuration);
            SubmissionResult = serviceRequestService.SubmissionResult;
        }
        public /*static*/ void PauseTilCompleted(Task vendorSubmit, int threadSleepDuration = 500)
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
        private async Task ProcessDefault()
        {
            await serviceRequestService.SubmitToVendor();
        }
        private async Task ProcessSubmissionToVendorXml()
        {
            await serviceRequestService.SubmitToVendor(ServiceRequestXml, __vendorUserId, __vendorId);
        }
        private async Task ProcessSubmissionToVendorJson()
        {
            await serviceRequestService.SubmitToVendor(ServiceRequestJson, __vendorUserId, __vendorId);
        }
    }
}
