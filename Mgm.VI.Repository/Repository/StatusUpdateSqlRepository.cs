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
    public class StatusUpdateSqlRepository : AdoRepositoryBase<IStatusUpdateModel>, IRepository<IStatusUpdateModel>
    {
        public StatusUpdateSqlRepository(string connectionString) : base(connectionString)
        {
        }
        public void Dispose()
        {
        }

        [Obsolete("Temporarily available until base functions prove successful")]
        public bool AddExplicit(IStatusUpdateModel statusUpdate)
        {
            bool addedSuccessfully = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string commandText = Db.StatusUpdate.Insert;
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    #region Command Parameters
                    command.Parameters.AddWithValue(Db.StatusUpdate.VendorId, statusUpdate.VendorId);
                    command.Parameters.AddWithValue(Db.StatusUpdate.VendorName, statusUpdate.VendorName);
                    command.Parameters.AddWithValue(Db.StatusUpdate.ServiceRequestId, statusUpdate.ServiceRequestId);
                    command.Parameters.AddWithValue(Db.StatusUpdate.Comments, statusUpdate.Comments);
                    command.Parameters.AddWithValue(Db.StatusUpdate.MessageContent, statusUpdate.MessageContent);
                    #endregion Command Parameters
                    try
                    {
                        #region Execution
                        var returnValue = (int) command.ExecuteScalar();
                        if (returnValue > 0)
                            addedSuccessfully = true;
                        else
                            ErrorMessages.Add("StatusUpdateSqlRepository::Add(IStatusUpdateModel)", 
                                                String.Format("Error returned: {0}", returnValue));
                        #endregion Execution
                    }
                    catch (Exception e)
                    {
                        ErrorMessages.Add("StatusUpdateSqlRepository::Add(IStatusUpdateModel)", e.Message, e);
                    }

                }
                connection.Close();
            }
            return addedSuccessfully;
        }
        public int Add(IStatusUpdateModel statusUpdate)
        {
            int id = 0;
            using (SqlCommand command = new SqlCommand(Db.StatusUpdate.Insert))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.StatusUpdate.VendorId, statusUpdate.VendorId);
                command.Parameters.AddWithValue(Db.StatusUpdate.VendorName, statusUpdate.VendorName);
                command.Parameters.AddWithValue(Db.StatusUpdate.ServiceRequestId, statusUpdate.ServiceRequestId);
                command.Parameters.AddWithValue(Db.StatusUpdate.Comments, statusUpdate.Comments);
                command.Parameters.AddWithValue(Db.StatusUpdate.MessageContent, statusUpdate.MessageContent);
                #endregion Command Parameters
                id = base.Add(command);
            }
            return id;
        }
        public int Persist_To_Mss(IStatusUpdateModel statusUpdate)
        {
            int updatedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.StatusUpdate.Persist_To_Mss))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.StatusUpdate.StatusUpdateId, statusUpdate.StatusUpdateId);
                #endregion Command Parameters
                updatedRecords = base.Update(command);
            }
            return updatedRecords;
        }
        public int Remove(string serviceRequestId)
        {
            int deletedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.StatusUpdate.Delete))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.StatusUpdate.ServiceRequestId, serviceRequestId);
                #endregion Command Parameters
                deletedRecords = base.Delete(command);
            }
            return deletedRecords;
        }
        public int Remove(int statusUpdateId) // Convert it to an Inactivate (SQL Update) statement
        {
            int deletedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.StatusUpdate.Delete))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.StatusUpdate.StatusUpdateId, statusUpdateId);
                #endregion Command Parameters
                deletedRecords = base.Delete(command);
            }
            return deletedRecords;
        }
        public int Update(IStatusUpdateModel statusUpdate)
        {
            int updatedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.StatusUpdate.Update))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.StatusUpdate.StatusUpdateId, statusUpdate.StatusUpdateId);
                command.Parameters.AddWithValue(Db.StatusUpdate.VendorId, statusUpdate.VendorId);
                command.Parameters.AddWithValue(Db.StatusUpdate.VendorName, statusUpdate.VendorName);
                command.Parameters.AddWithValue(Db.StatusUpdate.ServiceRequestId, statusUpdate.ServiceRequestId);
                command.Parameters.AddWithValue(Db.StatusUpdate.Comments, statusUpdate.Comments);
                command.Parameters.AddWithValue(Db.StatusUpdate.MessageContent, statusUpdate.MessageContent);
                #endregion Command Parameters
                updatedRecords = base.Update(command);
            }
            return updatedRecords;
        }
        public IStatusUpdateModel Get(string id)
        {
            if (!IsSqlSafe(StatusUpdate.TableName) || !IsSqlSafe(StatusUpdate.StatusUpdateId)) return null;

            var item = new StatusUpdateModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @id",
                                                StatusUpdate.TableName, StatusUpdate.StatusUpdateId)))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                item = (StatusUpdateModel) base.GetRecord<StatusUpdateModel, IStatusUpdateModel>(command);
            }
            return item;
        }
        public IEnumerable<IStatusUpdateModel> GetAllVendorRecords(string vendorId)
        {
            if (!IsSqlSafe(StatusUpdate.TableName) || !IsSqlSafe(StatusUpdate.VendorId)) return null;

            var items = new List<IStatusUpdateModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @VendorId", 
                                                StatusUpdate.TableName, StatusUpdate.VendorId)))
            {
                command.Parameters.Add(new SqlParameter("VendorId", vendorId));
                items = new List<IStatusUpdateModel>(base.GetRecords<StatusUpdateModel, IStatusUpdateModel>(command));
            }
            return items;
        }
        public IEnumerable<IStatusUpdateModel> GetAll()
        {
            if (!IsSqlSafe(StatusUpdate.TableName) || !IsSqlSafe(StatusUpdate.StatusUpdateId)) return null;

            var items = new List<IStatusUpdateModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0}", StatusUpdate.TableName)))
            {
                items = new List<IStatusUpdateModel>(base.GetRecords<StatusUpdateModel, IStatusUpdateModel>(command));
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