using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using Mgm.VI.Common;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Data.Model
{
    public abstract class ModelBase
    {
        public string Content { get; }
        public IErrorMessages ErrorMessages { get; private set; }
        public bool IsHydrated { get; protected set; }
        //public void LogErrors {}
        public delegate void FieldMapper(SqlDataReader reader, IModel model);

        public ModelBase(IErrorMessages errorMessages = null)
        {
            ErrorMessages = errorMessages ?? new ErrorMessages();//Use DI later to inject an instance
        }
        public string DumpContent()
        {
            return Content;
        }
        public static IModel FillModelFromReader(SqlDataReader reader, IModel model)
        {
            try
            {
                if (reader != null && model != null)
                {
                    //switch(model.GetType().ToString().ToLower())
                    //{
                    //    case "errorlogmodel":
                    //        FillModelFromReader(reader, (ErrorLogModel)model);
                    //        break;
                    //    case "statusupdatemodel":
                    //        FillModelFromReader(reader, (StatusUpdateModel)model);
                    //        break;
                    //    case "vendormodel":
                    //        FillModelFromReader(reader, (VendorModel)model);
                    //        break;
                    //    case "servicerequesthistorymodel":
                    //        FillModelFromReader(reader, (ServiceRequestHistoryModel)model);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    if (model is ErrorLogModel)
                        FillModelFromReader(reader, (ErrorLogModel)model);
                    else if (model is StatusUpdateModel)
                        FillModelFromReader(reader, (StatusUpdateModel)model);
                    else if (model is VendorModel)
                        FillModelFromReader(reader, (VendorModel)model);
                    else if (model is ServiceRequestHistoryModel)
                        FillModelFromReader(reader, (ServiceRequestHistoryModel)model);
                    else if (model is ServiceRequestStatusModel)
                        FillModelFromReader(reader, (ServiceRequestStatusModel)model);
                }
            }
            catch (Exception e)
            {
                //ErrorMessages.Add(String.Format("StatusUpdateSqlRepository::MapReaderToModel()"), e.Message, e);
            }
            return model;
        }
        public IT FillModelFromReader<IT>(SqlDataReader reader, IT model)
                                                                       where IT : IModel, new()
        {
            if (model != null)
                model = new IT();
            try
            {
                if (reader != null)
                {
                    FillModelFromReader(reader, model);
                }
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("StatusUpdateSqlRepository::MapReaderToModel()"), e.Message, e);
            }
            return model;
        }
        public static void FillModelFromReader(SqlDataReader reader, ErrorLogModel model)
        {
            if (reader != null && model != null)
            {
                FillModelFromReader<int>(reader, ErrorLog.LogId, model.LogId);
                FillModelFromReader<string>(reader, ErrorLog.ApplicationName, model.ApplicationName);
                FillModelFromReader<string>(reader, ErrorLog.ApplicationPath, model.ApplicationPath);
                FillModelFromReader<short>(reader, ErrorLog.ErrorSeverity, model.ErrorSeverity);
                FillModelFromReader<string>(reader, ErrorLog.ErrorSource, model.ErrorSource);
                FillModelFromReader<string>(reader, ErrorLog.ErrorMessage, model.ErrorMessage);
                FillModelFromReader<string>(reader, ErrorLog.MessageContent, model.MessageContent);
                FillModelFromReader<string>(reader, ErrorLog.LoggedTimestamp, model.LoggedTimestamp);
            }
        }
        public static void FillModelFromReader(SqlDataReader reader, ServiceRequestHistoryModel model)
        {
            if (reader != null && model != null)
            {
                FillModelFromReader<int>(reader,    ServiceRequestHistory.Id, model.Id);
                FillModelFromReader<string>(reader, ServiceRequestHistory.ServiceRequestId, model.ServiceRequestId);
                FillModelFromReader<string>(reader, ServiceRequestHistory.VendorId, model.VendorId);
                FillModelFromReader<string>(reader, ServiceRequestHistory.VendorName, model.VendorName);
                FillModelFromReader<string>(reader, ServiceRequestHistory.Comments, model.Comments);
                FillModelFromReader<string>(reader, ServiceRequestHistory.MessageContent, model.MessageContent);
                FillModelFromReader<string>(reader, ServiceRequestHistory.SubmissionTimestamp, model.SubmissionTimestamp);
            }
        }
        public static void FillModelFromReader(SqlDataReader reader, ServiceRequestStatusModel model)
        {
            if (reader != null && model != null)
            {
                FillModelFromReader<int>(reader, ServiceRequestStatus.Id, model.Id);
                FillModelFromReader<bool>(reader, ServiceRequestStatus.Active, model.Active);
                FillModelFromReader<string>(reader, ServiceRequestStatus.Comments, model.Comments);
                FillModelFromReader<string>(reader, ServiceRequestStatus.CreateDate, model.CreateDate);
                FillModelFromReader<string>(reader, ServiceRequestStatus.CreatedBy, model.CreatedBy);
                FillModelFromReader<string>(reader, ServiceRequestStatus.MasterDataCode, model.MasterDataCode);
                FillModelFromReader<string>(reader, ServiceRequestStatus.MasterDataName, model.MasterDataName);
                FillModelFromReader<string>(reader, ServiceRequestStatus.MasterDataValue, model.MasterDataValue);
                FillModelFromReader<string>(reader, ServiceRequestStatus.SequenceOrder, model.SequenceOrder);
                FillModelFromReader<string>(reader, ServiceRequestStatus.UpdateDate, model.UpdateDate);
            }
        }
        public static void FillModelFromReader(SqlDataReader reader, StatusUpdateModel model)
        {
            if (reader != null && model != null)
            {
                FillModelFromReader<int>(reader, StatusUpdate.StatusUpdateId, model.StatusUpdateId);
                FillModelFromReader<string>(reader, StatusUpdate.ServiceRequestId, model.ServiceRequestId);
                FillModelFromReader<string>(reader, StatusUpdate.VendorId, model.VendorId);
                FillModelFromReader<string>(reader, StatusUpdate.VendorName, model.VendorName);
                FillModelFromReader<string>(reader, StatusUpdate.Comments, model.Comments);
                FillModelFromReader<string>(reader, StatusUpdate.MessageContent, model.MessageContent);
                FillModelFromReader<string>(reader, StatusUpdate.SqsRetrievalTimestamp, model.SqsRetrievalTimestamp);
                FillModelFromReader<bool>(reader, StatusUpdate.IsPersistedToMss, model.IsPersistedToMss);
            }
        }
        public static void FillModelFromReader(SqlDataReader reader, VendorModel model)
        {
            if (reader != null && model != null)
            {
                FillModelFromReader<int>(reader, Vendor.StagingId, model.StagingId);
                FillModelFromReader<string>(reader, Vendor.VendorId, model.VendorId);
                FillModelFromReader<string>(reader, Vendor.VendorName, model.VendorName);
                FillModelFromReader<string>(reader, Vendor.ServiceRequestApiURI, model.ServiceRequestApiURI);
                FillModelFromReader<string>(reader, Vendor.AWS_AccessKey, model.AWS_AccessKey);
                FillModelFromReader<string>(reader, Vendor.AWS_SecretKey, model.AWS_SecretKey);
                FillModelFromReader<string>(reader, Vendor.SQS_RegionEndPoint, model.SQS_RegionEndPoint);
                FillModelFromReader<string>(reader, Vendor.SQS_ServiceURL, model.SQS_ServiceURL);
                FillModelFromReader<string>(reader, Vendor.SQS_StatusUpdateDeadLetterURI, model.SQS_StatusUpdateDeadLetterURI);
                FillModelFromReader<string>(reader, Vendor.SQS_StatusUpdatePrimaryURI, model.SQS_StatusUpdatePrimaryURI);
                FillModelFromReader<string>(reader, Vendor.NotificationRecepients, model.NotificationRecepients);
                FillModelFromReader<string>(reader, Vendor.LastUpdatedBy, model.LastUpdatedBy);
                FillModelFromReader<string>(reader, Vendor.LastUpdatedOn, model.LastUpdatedOn);
            }
        }
        public static void FillModelFromReader<IT>(SqlDataReader reader,
                                                    string columnName, IT field)
        {
            switch (field.GetType().Name.ToLower())
            {
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "short":
                case "uint":
                case "ulong":
                case "ushort":
                    field = (IT)(reader[columnName] ?? 0);
                    break;
                case "decimal":
                case "double":
                case "float":
                    field = (IT)(reader[columnName] ?? 0.0);
                    break;
                default:
                    field = (IT)(reader[columnName] ?? "");
                    break;
                case "bit":
                    field = (IT)(reader[columnName] ?? 0);
                    break;
            }
        }
    }
}
