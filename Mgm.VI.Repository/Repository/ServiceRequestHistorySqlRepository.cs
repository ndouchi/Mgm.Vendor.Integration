//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using Mgm.VI.Repository;
using static Mgm.VI.Common.Constants;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Repository
{
    public class ServiceRequestHistorySqlRepository : AdoRepositoryBase<IServiceRequestHistoryModel>, IRepository<IServiceRequestHistoryModel>
    {
        public ServiceRequestHistorySqlRepository(string connectionString) : base(connectionString)
        {
        }
        public void Dispose()
        {
        }
        public int Add(IServiceRequestHistoryModel serviceRequestHistory)
        {
            int id = 0;
            using (SqlCommand command = new SqlCommand(Db.ServiceRequestHistory.Insert))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.ServiceRequestId, serviceRequestHistory.ServiceRequestId);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.VendorId, serviceRequestHistory.VendorId);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.VendorName, serviceRequestHistory.VendorName);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.Comments, serviceRequestHistory.Comments);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.MessageContent, serviceRequestHistory.MessageContent);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.SubmissionTimestamp, serviceRequestHistory.SubmissionTimestamp);
                #endregion Command Parameters
                id = base.Add(command);
            }
            return id;
        }
        public int Remove(string serviceRequestId)
        {
            if (!IsSqlSafe(ServiceRequestHistory.TableName) || !IsSqlSafe(ServiceRequestHistory.Id)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @ServiceRequestId",
                                                    ServiceRequestHistory.TableName, ServiceRequestHistory.ServiceRequestId)))
            {
                command.Parameters.Add(new SqlParameter("ServiceRequestId", serviceRequestId));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Remove(int Id)
        {
            if (!IsSqlSafe(ServiceRequestHistory.TableName) || !IsSqlSafe(ServiceRequestHistory.Id)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @serviceRequestId",
                                                    ServiceRequestHistory.TableName, ServiceRequestHistory.Id)))
            {
                command.Parameters.Add(new SqlParameter("Id", Id));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Update(IServiceRequestHistoryModel serviceRequestHistory)
        {
            int id = 0;
            using (SqlCommand command = new SqlCommand(Db.ServiceRequestHistory.Update))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.Id, serviceRequestHistory.Id);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.ServiceRequestId, serviceRequestHistory.ServiceRequestId);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.VendorId, serviceRequestHistory.VendorId);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.VendorName, serviceRequestHistory.VendorName);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.Comments, serviceRequestHistory.Comments);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.MessageContent, serviceRequestHistory.MessageContent);
                command.Parameters.AddWithValue(Db.ServiceRequestHistory.SubmissionTimestamp, serviceRequestHistory.SubmissionTimestamp);
                #endregion Command Parameters
                id = base.Update(command);
            }
            return id;
        }
        public IServiceRequestHistoryModel Get(string id)
        {
            if (!IsSqlSafe(ServiceRequestHistory.TableName) || !IsSqlSafe(ServiceRequestHistory.Id)) return null;

            var item = new ServiceRequestHistoryModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @id",
                                                ServiceRequestHistory.TableName, ServiceRequestHistory.Id)))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                item = (ServiceRequestHistoryModel)GetRecord<ServiceRequestHistoryModel, IServiceRequestHistoryModel>(command);
            }
            return item;
        }
        public IEnumerable<IServiceRequestHistoryModel> GetAllVendorRecords(string vendorId)
        {
            if (!IsSqlSafe(ServiceRequestHistory.TableName) || !IsSqlSafe(ServiceRequestHistory.VendorId)) return null;

            var items = new List<IServiceRequestHistoryModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @VendorId",
                                                ServiceRequestHistory.TableName, ServiceRequestHistory.VendorId)))
            {
                command.Parameters.Add(new SqlParameter("VendorId", vendorId));
                items = new List<IServiceRequestHistoryModel>(base.GetRecords<ServiceRequestHistoryModel, IServiceRequestHistoryModel>(command));
            }
            return items;
        }
        public IEnumerable<IServiceRequestHistoryModel> GetAll()
        {
            if (!IsSqlSafe(ServiceRequestHistory.TableName) || !IsSqlSafe(ServiceRequestHistory.Id)) return null;

            var items = new List<IServiceRequestHistoryModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0}", ServiceRequestHistory.TableName)))
            {
                items = new List<IServiceRequestHistoryModel>(GetRecords<ServiceRequestHistoryModel, IServiceRequestHistoryModel>(command));
            }
            return items;
        }
        public bool Save()
        {
            bool savedSuccessfully = false;
            return savedSuccessfully;
        }
    }
}