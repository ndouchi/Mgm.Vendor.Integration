
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Data.Model
{
    public class ServiceRequestHistoryModel : ModelBase, IServiceRequestHistoryModel
    {
        public int Id { get; set; }
        public string ServiceRequestId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string Comments { get; set; }
        public string MessageContent { get; set; }
        public string SubmissionTimestamp { get; set; }
        public IServiceRequestHistoryModel FillModelFromReader(SqlDataReader reader)
        {
            var model = new ServiceRequestHistoryModel();
            try
            {
                if (reader.HasRows && reader.Read())
                    FillModelFromReader(reader, model);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("ServiceRequestHistorySqlRepository::MapReaderToModel()"), e.Message, e);
            }
            return model;
        }
    }
}
