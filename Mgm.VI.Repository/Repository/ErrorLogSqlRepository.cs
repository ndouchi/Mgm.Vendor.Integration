//////////////////////////////////////////////////////////////////
using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using static Mgm.VI.Common.Constants;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Repository
{
    public class ErrorLogSqlRepository : AdoRepositoryBase<IErrorLogModel>, IRepository<IErrorLogModel>
    {
        public ErrorLogSqlRepository(string connectionString) : base(connectionString)
        {}
        public void Dispose()
        {}
        public int Add(IErrorLogModel errorLog)
        {
            int id = 0;
            using (SqlCommand command = new SqlCommand(Db.ErrorLog.Insert))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.ErrorLog.ApplicationName, errorLog.ApplicationName);
                command.Parameters.AddWithValue(Db.ErrorLog.ApplicationPath, errorLog.ApplicationPath);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorMessage, errorLog.ErrorMessage);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorSeverity, errorLog.ErrorSeverity);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorSource, errorLog.ErrorSource);
                command.Parameters.AddWithValue(Db.ErrorLog.MessageContent, errorLog.MessageContent);
                #endregion Command Parameters
                id = base.Add(command);
            }
            return id;
        }
        public int Remove(string loggedTimestamp)
        {
            if (!IsSqlSafe(ErrorLog.TableName) || !IsSqlSafe(ErrorLog.LogId)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @LoggedTimestamp",
                                                    ErrorLog.TableName, ErrorLog.LoggedTimestamp)))
            {
                command.Parameters.Add(new SqlParameter("LoggedTimestamp", loggedTimestamp));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Remove(int logId)
        {
            if (!IsSqlSafe(ErrorLog.TableName) || !IsSqlSafe(ErrorLog.LogId)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @LogId",
                                                    ErrorLog.TableName, ErrorLog.LogId)))
            {
                command.Parameters.Add(new SqlParameter("LogId", logId));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Update(IErrorLogModel errorLogModel)
        {
            int updatedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.Vendor.Update))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.ErrorLog.LogId, errorLogModel.LogId);
                command.Parameters.AddWithValue(Db.ErrorLog.ApplicationName, errorLogModel.ApplicationName);
                command.Parameters.AddWithValue(Db.ErrorLog.ApplicationName, errorLogModel.ApplicationName);
                command.Parameters.AddWithValue(Db.ErrorLog.ApplicationPath, errorLogModel.ApplicationPath);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorMessage, errorLogModel.ErrorMessage);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorSeverity, errorLogModel.ErrorSeverity);
                command.Parameters.AddWithValue(Db.ErrorLog.ErrorSource, errorLogModel.ErrorSource);
                command.Parameters.AddWithValue(Db.ErrorLog.MessageContent, errorLogModel.MessageContent);
                command.Parameters.AddWithValue(Db.ErrorLog.LoggedTimestamp, errorLogModel.LoggedTimestamp);
                #endregion Command Parameters
                updatedRecords = base.Update(command);
            }
            return updatedRecords;
        }
        public IErrorLogModel Get(string logId)
        {
            if (!IsSqlSafe(ErrorLog.TableName) || !IsSqlSafe(ErrorLog.LogId)) return null;

            var item = new ErrorLogModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @LogId",
                                                ErrorLog.TableName, ErrorLog.LogId)))
            {
                command.Parameters.Add(new SqlParameter("LogId", logId));
                item = (ErrorLogModel)GetRecord<ErrorLogModel, IErrorLogModel>(command);
            }
            return item;
        }
        public IEnumerable<IErrorLogModel> GetAllApplicationErrorLogRecords(string applicationName)
        {
            if (!IsSqlSafe(ErrorLog.TableName) || !IsSqlSafe(ErrorLog.ApplicationName)) return null;

            var items = new List<IErrorLogModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @ApplicationName",
                                                ErrorLog.TableName, ErrorLog.ApplicationName)))
            {
                command.Parameters.Add(new SqlParameter("ApplicationName", applicationName));
                items = new List<IErrorLogModel>(base.GetRecords<ErrorLogModel, IErrorLogModel>(command));
            }
            return items;
        }
        public IEnumerable<IErrorLogModel> GetAll()
        {
            if (!IsSqlSafe(ErrorLog.TableName) || !IsSqlSafe(ErrorLog.LogId)) return null;

            var items = new List<IErrorLogModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0}", ErrorLog.TableName)))
            {
                items = new List<IErrorLogModel>(GetRecords<ErrorLogModel, IErrorLogModel>(command));
            }
            return items;
        }
        public bool Save()
        {
            return true;
        }
    }
}