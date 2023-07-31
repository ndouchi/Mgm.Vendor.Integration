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
    public class VendorSqlRepository : AdoRepositoryBase<IVendorModel>, IRepository<IVendorModel>
    {
        private string connectionString;
        public VendorSqlRepository(string connectionString) : base(connectionString)
        {
        }
        public void Dispose()
        {
        }
        public int Add(IVendorModel vendor)
        {
            int id = 0;
            using (SqlCommand command = new SqlCommand(Db.Vendor.Insert))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.Vendor.VendorId, vendor.VendorId);
                command.Parameters.AddWithValue(Db.Vendor.VendorName, vendor.VendorName);
                command.Parameters.AddWithValue(Db.Vendor.AWS_AccessKey, vendor.AWS_AccessKey);
                command.Parameters.AddWithValue(Db.Vendor.AWS_SecretKey, vendor.AWS_SecretKey);
                command.Parameters.AddWithValue(Db.Vendor.ServiceRequestApiURI, vendor.ServiceRequestApiURI);
                command.Parameters.AddWithValue(Db.Vendor.SQS_RegionEndPoint, vendor.SQS_RegionEndPoint);
                command.Parameters.AddWithValue(Db.Vendor.SQS_ServiceURL, vendor.SQS_ServiceURL);
                command.Parameters.AddWithValue(Db.Vendor.SQS_StatusUpdateDeadLetterURI, vendor.SQS_StatusUpdateDeadLetterURI);
                command.Parameters.AddWithValue(Db.Vendor.SQS_StatusUpdatePrimaryURI, vendor.SQS_StatusUpdatePrimaryURI);
                #endregion Command Parameters
                id = base.Add(command);
            }
            return id;
        }
        public int Remove(string vendorId)
        {
            if (!IsSqlSafe(Vendor.TableName) || !IsSqlSafe(Vendor.VendorId)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @VendorId",
                                                    Vendor.TableName, Vendor.VendorId)))
            {
                command.Parameters.Add(new SqlParameter("VendorId", vendorId));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Remove(int stagingId)
        {
            if (!IsSqlSafe(Vendor.TableName) || !IsSqlSafe(Vendor.StagingId)) return Db.Unsafe;

            int deletedRecords = 0;
            using (var command =
                    new SqlCommand(String.Format("DELETE FROM {0} WHERE {1} = @StagingId",
                                                    Vendor.TableName, Vendor.StagingId)))
            {
                command.Parameters.Add(new SqlParameter("StagingId", stagingId));
                deletedRecords = Delete(command);
            }
            return deletedRecords;
        }
        public int Update(IVendorModel vendor)
        {
            int updatedRecords = 0;
            using (SqlCommand command = new SqlCommand(Db.Vendor.Update))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                #region Command Parameters
                command.Parameters.AddWithValue(Db.Vendor.VendorId, vendor.VendorId);
                command.Parameters.AddWithValue(Db.Vendor.VendorName, vendor.VendorName);
                command.Parameters.AddWithValue(Db.Vendor.AWS_AccessKey, vendor.AWS_AccessKey);
                command.Parameters.AddWithValue(Db.Vendor.AWS_SecretKey, vendor.AWS_SecretKey);
                command.Parameters.AddWithValue(Db.Vendor.ServiceRequestApiURI, vendor.ServiceRequestApiURI);
                command.Parameters.AddWithValue(Db.Vendor.SQS_RegionEndPoint, vendor.SQS_RegionEndPoint);
                command.Parameters.AddWithValue(Db.Vendor.SQS_ServiceURL, vendor.SQS_ServiceURL);
                command.Parameters.AddWithValue(Db.Vendor.SQS_StatusUpdateDeadLetterURI, vendor.SQS_StatusUpdateDeadLetterURI);
                command.Parameters.AddWithValue(Db.Vendor.SQS_StatusUpdatePrimaryURI, vendor.SQS_StatusUpdatePrimaryURI);
                #endregion Command Parameters
                updatedRecords = base.Update(command);
            }
            return updatedRecords;
        }
        public IVendorModel Get(string vendorId)
        {
            if (!IsSqlSafe(Vendor.TableName) || !IsSqlSafe(Vendor.VendorId)) return null;

            var item = new VendorModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @VendorId",
                                                Vendor.TableName, Vendor.VendorId)))
            {
                command.Parameters.Add(new SqlParameter("VendorId", vendorId));
                item = (VendorModel)GetRecord<VendorModel, IVendorModel>(command);
            }
            return item;
        }
        public IVendorModel GetByVendorName(string vendorName)
        {
            if (!IsSqlSafe(Vendor.TableName) || !IsSqlSafe(Vendor.VendorName)) return null;

            var item = new VendorModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @VendorName",
                                                Vendor.TableName, Vendor.VendorName)))
            {
                command.Parameters.Add(new SqlParameter("VendorName", vendorName));
                item = (VendorModel)GetRecord<VendorModel, IVendorModel>(command);
            }
            return item;
        }
        public IEnumerable<IVendorModel> GetAll()
        {
            if (!IsSqlSafe(Vendor.TableName)) return null;

            var items = new List<IVendorModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0}", Vendor.TableName)))
            {
                items = new List<IVendorModel>(GetRecords<VendorModel, IVendorModel>(command));
            }
            return items;
        }
        public bool Save()
        {
            return true;
        }
    }
}