using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Common
{
    public class Constants
    {
        public static class Db
        {
            public static int Unsafe = -999;

            public static class ErrorLog
            {
                public const string TableName = "Error_Log";

                #region Stored Procedures
                public const string Insert = "sp_Add_Error_Log";
                public const string List = "sp_Get_Error_Log";
                public const string Update = "sp_Update_Error_Log";
                #endregion Stored Procedures

                #region Table Columns
                public const string LogId = "LogId";
                public const string ApplicationName = "ApplicationName";
                public const string ApplicationPath = "ApplicationPath";
                public const string ErrorSeverity = "ErrorSeverity";
                public const string ErrorSource = "ErrorSource";
                public const string ErrorMessage = "ErrorMessage";
                public const string MessageContent = "MessageContent";
                public const string LoggedTimestamp = "LoggedTimestamp";
                #endregion Table Columns
            }
            public static class ServiceRequestHistory
            {
                public const string TableName = "Status_Update_History";

                #region Stored Procedures
                public const string Insert = "sp_Add_Service_Request_History";
                public const string Deactivate = "sp_Deactivate_Service_Request_History";
                public const string List = "sp_Get_Service_Request_History";
                public const string Update = "sp_Update_Service_Request_History";
                #endregion Stored Procedures

                #region Table Columns
                public const string Id = "Id";
                public const string ServiceRequestId = "ServiceRequestId";
                public const string VendorId = "VendorId";
                public const string VendorName = "VendorName";
                public const string Comments = "Comments";
                public const string MessageContent = "MessageContent";
                public const string SubmissionTimestamp = "SubmissionTimestamp";
                #endregion Table Columns
            }
            public static class ServiceRequestStatus
            {
                public const string TableName = "MSS_SR_STATUS_MASTER";

                #region Stored Procedures
                public const string List = "sp_Get_Service_Request_Status";
                #endregion Stored Procedures

                #region Table Columns
                public const string Id = "ID";
                public const string MasterDataName = "MASTER_DATA_NAME";
                 public const string MasterDataCode = "MASTER_DATA_CODE";
                public const string MasterDataValue = "MASTER_DATA_VALUE";
                public const string SequenceOrder = "SEQUENCE_ORDER";
                public const string CreatedBy = "CREATED_BY";
                public const string Comments = "COMMENTS";
                public const string Active = "ACTIVE";
                public const string CreateDate = "CREATE_DATE";
                public const string UpdateDate = "UPDATE_DATE";

                #endregion Table Columns
            }
            public static class StatusUpdate
            {
                public const string TableName = "Status_Update";

                #region Stored Procedures
                public const string Delete = "sp_Delete_Status_Update";
                public const string Insert = "sp_Add_Status_Update";
                public const string List = "sp_Get_Status_Updates";
                public const string Persist_To_Mss = "sp_Persist_To_Mss";
                public const string Update = "sp_Update_Status_Update";
                #endregion Stored Procedures

                #region Table Columns
                public const string StatusUpdateId = "StatusUpdateId";
                public const string VendorId = "VendorId";
                public const string VendorName = "VendorName";
                public const string ServiceRequestId = "ServiceRequestId";
                public const string Comments = "Comments";
                public const string MessageContent = "MessageContent";
                public const string SqsRetrievalTimestamp = "SqsRetrievalTimestamp";
                public const string IsPersistedToMss = "IsPersistedToMss";
                #endregion Table Columns
            }
            public static class Vendor
            {
                public const string TableName = "Vendor";

                #region Stored Procedures
                public const string Insert = "sp_Add_Vendor";
                public const string List = "sp_Get_Vendors";
                public const string Update = "sp_Update_Vendor";
                #endregion Stored Procedures

                #region Table Columns
                public const string VendorId = "VendorId";
                public const string VendorName = "VendorName";
                public const string StagingId = "StagingId";
                public const string AWS_AccessKey = "AWS_AccessKey";
                public const string AWS_SecretKey = "AWS_SecretKey";
                public const string ServiceRequestApiURI = "ServiceRequestApiURI";
                public const string SQS_RegionEndPoint = "SQS_RegionEndPoint";
                public const string SQS_ServiceURL = "SQS_ServiceURL";
                public const string SQS_StatusUpdateDeadLetterURI = "SQS_StatusUpdateDeadLetterURI";
                public const string SQS_StatusUpdatePrimaryURI = "SQS_StatusUpdatePrimaryURI";
                public const string NotificationRecepients = "NotificationRecepients";
                public const string LastUpdatedBy = "LastUpdatedBy";
                public const string LastUpdatedOn = "LastUpdatedOn";
                #endregion Table Columns
            }
        }
        public class MssXsd
        {
            public const string ServiceRequest = "ServiceRequest";
            public const string StatusUpdate = "StatusUpdate";
            public const string ErrorLog = "ErrorLog";
            public const string Vendor = "Vendor";
        }
    }
}
