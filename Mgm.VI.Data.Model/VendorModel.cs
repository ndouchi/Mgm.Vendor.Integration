using System;
using System.Data.SqlClient;

namespace Mgm.VI.Data.Model
{
    public class VendorModel : ModelBase, IVendorModel
    {
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


        public IVendorModel FillModelFromReader(SqlDataReader reader)
        {
            var model = new VendorModel();
            try
            {
                if (reader.HasRows && reader.Read())
                    FillModelFromReader(reader, model);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("VendorModel::FillModelFromReader()"), e.Message, e);
            }
            return model;
        }
    }
}
