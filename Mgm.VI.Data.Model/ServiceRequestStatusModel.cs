
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Data.Model
{
    public class ServiceRequestStatusModel : ModelBase, IServiceRequestStatusModel
    {
        #region Properties
        public int Id { get; set; }
        public string MasterDataName { get; set; }
        public string MasterDataCode { get; set; }
        public string MasterDataValue { get; set; }
        public string SequenceOrder { get; set; }
        public string CreatedBy { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        #endregion Properties
        public IServiceRequestStatusModel FillModelFromReader(SqlDataReader reader)
        {
            var model = new ServiceRequestStatusModel();
            try
            {
                if (reader.HasRows && reader.Read())
                    FillModelFromReader(reader, model);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("ServiceRequestStatusSqlRepository::MapReaderToModel()"), e.Message, e);
            }
            return model;
        }
    }
}
