using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mgm.VI.Services
{
    public interface IServiceRequestService : IService, IDisposable
    {
        ServiceRequestSubmitResult SubmissionResult { get; }
        /*static*/
        void PauseTilCompleted(Task vendorSubmit, int threadSleepDuration = 500);
        void PauseTilCompleted(int threadSleepDuration = 500);
        Task SubmitToVendor();
        Task SubmitToVendor(XDocument serviceRequestXml, string userId, string vendorId);
        Task SubmitToVendor(string serviceRequestJson, string userId, string vendorId);
        IServiceRequestService SubmitServiceRequestToVendor();
        IServiceRequestService SubmitServiceRequestToVendor(XDocument xmlDoc, string vendorUser, string vendorId);
        IServiceRequestService SubmitServiceRequestToVendor(string serviceRequestJson, string vendorUser, string vendorId);
    }
}
