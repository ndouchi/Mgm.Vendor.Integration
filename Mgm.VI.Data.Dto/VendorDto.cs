using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Mgm.VI.Common;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto 
{
    public class VendorDto : DtoBase, IVendorDto
    {
        public static readonly string XmlFieldName = "Vendor";
        public static readonly string XmlGroupingFieldName = "Vendors";

        #region Properties
        public int StagingId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ServiceRequestApiURI { get; set; }
        public string SQS_StatusUpdatePrimaryURI { get; set; }
        public string SQS_StatusUpdateDeadLetterURI { get; set; }
        public string SQS_ServiceURL { get; set; }
        public string SQS_RegionEndPoint { get; set; }

        public string AWS_AccessKey { get; set; }
        public string AWS_SecretKey { get; set; }
        public string NotificationRecepients { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedOn { get; set; }
        #endregion Properties


        private void Initialize(int stagingId, string vendorId, string vendorName,
                                string serviceRequestApiURI,
                                string sQS_StatusUpdatePrimaryURI,
                                string sQS_StatusUpdateDeadLetterURI,
                                string sQS_ServiceURL,
                                string sQS_RegionEndPoint,
                                string aWS_AccessKey,
                                string aWS_SecretKey,
                                string notificationRecepients,
                                string lastUpdatedBy,
                                string lastUpdatedOn
      )
        {
            try
            {
                #region Initialize Properties
                StagingId = stagingId;
                VendorId = vendorId;
                VendorName = vendorName;
                ServiceRequestApiURI = serviceRequestApiURI;
                SQS_StatusUpdatePrimaryURI = sQS_StatusUpdatePrimaryURI;
                SQS_StatusUpdateDeadLetterURI = sQS_StatusUpdateDeadLetterURI;
                SQS_ServiceURL = sQS_ServiceURL;
                SQS_RegionEndPoint = sQS_RegionEndPoint;
                AWS_AccessKey = aWS_AccessKey;
                AWS_SecretKey = aWS_SecretKey;
                NotificationRecepients = notificationRecepients;
                LastUpdatedBy = lastUpdatedBy;
                LastUpdatedOn = lastUpdatedOn;

                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("VendorDto::Initialize(...)", ex.Message, ex);
            }
        }

        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            throw new NotImplementedException();
        }
        #region Constructors
        public VendorDto() : base() { }
        public VendorDto(int stagingId, string vendorId, string vendorName,
                                string serviceRequestApiURI,
                                string sQS_StatusUpdatePrimaryURI,
                                string sQS_StatusUpdateDeadLetterURI,
                                string sQS_ServiceURL,
                                string sQS_RegionEndPoint,
                                string aWS_AccessKey,
                                string aWS_SecretKey,
                                string notificationRecepients,
                                string lastUpdatedBy,
                                string lastUpdatedOn) : this()
        {
            Initialize(stagingId, vendorId, vendorName,
                                serviceRequestApiURI,
                                sQS_StatusUpdatePrimaryURI,
                                sQS_StatusUpdateDeadLetterURI,
                                sQS_ServiceURL,
                                sQS_RegionEndPoint,
                                aWS_AccessKey,
                                aWS_SecretKey,
                                notificationRecepients,
                                lastUpdatedBy,
                                lastUpdatedOn );
        }
        #endregion Constructors

        #region Static Parsers
        #endregion Static Parsers
    }
}
