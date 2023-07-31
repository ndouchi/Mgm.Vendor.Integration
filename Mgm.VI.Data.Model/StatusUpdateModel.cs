
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Data.Model
{
    public class StatusUpdateModel : ModelBase, IStatusUpdateModel
    {
        public int StatusUpdateId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ServiceRequestId { get; set; }
        public string Comments { get; set; }
        public string MessageContent { get; set; }
        public string SqsRetrievalTimestamp { get; set; }
        public bool IsPersistedToMss { get; set; }
        public IStatusUpdateModel FillModelFromReader(SqlDataReader reader)
        {
            var model = new StatusUpdateModel();
            try
            {
                if (reader.HasRows && reader.Read()) 
                    FillModelFromReader(reader, model);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("StatusUpdateModel::FillModelFromReader()"), e.Message, e);
            }
            return model;
        }
    }
}
