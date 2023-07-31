using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mgm.VI.Facade
{
    public interface IServiceRequestFacade : IFacade
    {
        ServiceRequestSubmitResult SubmissionResult { get; }
        /*static*/
        void PauseTilCompleted(Task vendorSubmit, int threadSleepDuration = 500);
        void PauseTilCompleted(int threadSleepDuration = 500);
        Task SubmitToVendor();
        Task SubmitToVendor(XDocument serviceRequestXml, string userId, string vendorId);
        Task SubmitToVendor(string serviceRequestJson, string userId, string vendorId);
        IServiceRequestFacade SubmitServiceRequestToVendor();
        IServiceRequestFacade SubmitServiceRequestToVendor(XDocument xmlDoc, string vendorUser, string vendorId);
        IServiceRequestFacade SubmitServiceRequestToVendor(string serviceRequestJson, string vendorUser, string vendorId);
    }
}
