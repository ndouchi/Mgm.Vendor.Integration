using System;

namespace Mgm.VI.Data.Model
{
    public interface IVendorModel : IModel
    {
        #region Properties
        int StagingId { get; set; }
        string VendorId { get; set; }
        string VendorName { get; set; }
        string ServiceRequestApiURI { get; set; }
        string SQS_StatusUpdatePrimaryURI { get; set; }
        string SQS_StatusUpdateDeadLetterURI { get; set; }
        string SQS_ServiceURL { get; set; }
        string SQS_RegionEndPoint { get; set; }

        string AWS_AccessKey { get; set; }
        string AWS_SecretKey { get; set; }
        string NotificationRecepients { get; set; }
        string LastUpdatedBy { get; set; }
        string LastUpdatedOn { get; set; }
        #endregion Properties
    }
}
