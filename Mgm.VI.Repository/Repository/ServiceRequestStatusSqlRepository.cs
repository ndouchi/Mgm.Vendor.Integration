//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using static Mgm.VI.Common.Constants;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Repository
{
    public class ServiceRequestStatusSqlRepository : AdoRepositoryBase<IServiceRequestStatusModel>, IRepository<IServiceRequestStatusModel>
    {
        public ServiceRequestStatusSqlRepository(string connectionString) : base(connectionString)
        {
        }
        public void Dispose()
        {
        }
        public IServiceRequestStatusModel Get(string id)
        {
            if (!IsSqlSafe(ServiceRequestStatus.TableName) || !IsSqlSafe(ServiceRequestStatus.Id)) return null;

            var item = new ServiceRequestStatusModel();
            using (var command =
                    new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @id",
                                                ServiceRequestStatus.TableName, ServiceRequestStatus.Id)))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                item = (ServiceRequestStatusModel)GetRecord<ServiceRequestStatusModel, IServiceRequestStatusModel>(command);
            }
            return item;
        }
        public IEnumerable<IServiceRequestStatusModel> GetAllStatusRecords(string id)
        {
            if (!IsSqlSafe(ServiceRequestStatus.TableName) || !IsSqlSafe(ServiceRequestStatus.Id)) return null;

            var items = new List<IServiceRequestStatusModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE {1} = @Id",
                                                ServiceRequestStatus.TableName, ServiceRequestStatus.Id)))
            {
                command.Parameters.Add(new SqlParameter("Id", id));
                items = new List<IServiceRequestStatusModel>(base.GetRecords<ServiceRequestStatusModel, IServiceRequestStatusModel>(command));
            }
            return items;
        }
        public IEnumerable<IServiceRequestStatusModel> GetAll()
        {
            if (!IsSqlSafe(ServiceRequestStatus.TableName) || !IsSqlSafe(ServiceRequestStatus.Id)) return null;

            var items = new List<IServiceRequestStatusModel>();
            using (var command = new SqlCommand(String.Format("SELECT * FROM {0} WHERE MASTER_DATA_NAME='PPS'", ServiceRequestStatus.TableName)))
            {
                items = new List<IServiceRequestStatusModel>(GetRecords<ServiceRequestStatusModel, IServiceRequestStatusModel>(command));
            }
            return items;
        }
        public int Add(IServiceRequestStatusModel statusUpdateModel)
        {
            throw new NotImplementedException();
        }

        public int Remove(string id)
        {
            throw new NotImplementedException();
        }

        public int Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public int Update(IServiceRequestStatusModel statusUpdateModel)
        {
            throw new NotImplementedException();
        }
    }
}