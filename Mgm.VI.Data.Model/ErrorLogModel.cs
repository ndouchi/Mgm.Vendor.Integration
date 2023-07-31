using System;
using System.Data.SqlClient;

namespace Mgm.VI.Data.Model
{
    public class ErrorLogModel : ModelBase, IErrorLogModel
    {
        public int LogId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public short ErrorSeverity { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageContent { get; set; }
        public string LoggedTimestamp { get; set; }
        public IErrorLogModel FillModelFromReader(SqlDataReader reader)
        {
            var model = new ErrorLogModel();
            try
            {
                if (reader.HasRows && reader.Read())
                    FillModelFromReader(reader, model);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("ErrorLogModel::FillModelFromReader()"), e.Message, e);
            }
            return model;
        }
    }
}
