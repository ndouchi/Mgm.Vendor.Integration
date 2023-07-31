using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Map;
using Mgm.VI.Data.Model;
using Mgm.VI.Common;

namespace Mgm.VI.Repository
{
    public class ServiceRequestStatusUnitOfWork : UowBase, IServiceRequestStatusUnitOfWork
    {
        private bool disposed = false;
        private Storage storage;
        private ServiceRequestStatusSqlRepository serviceRequestStatusRepository;
        private ErrorLogSqlRepository errorLogSqlRepository;

        private List<IServiceRequestStatusDto> serviceRequestStatusDtos;
        public ErrorLogSqlRepository ErrorLogRepository
        {
            get
            {
                return this.errorLogSqlRepository ?? new ErrorLogSqlRepository(storage.ConnectionString);
            }
        }

        public List<IServiceRequestStatusDto> ServiceRequestStatusDtos
        {
            get
            {
                return this.serviceRequestStatusDtos ?? new List<IServiceRequestStatusDto>();
            }
            set
            {
                this.serviceRequestStatusDtos = value;
            }
        }

        public ServiceRequestStatusUnitOfWork(Storage store)
        {
            storage = store;
            serviceRequestStatusRepository = new ServiceRequestStatusSqlRepository(storage.ConnectionString);
        }
        public IServiceRequestStatusDto GetStatus(string statusUpdateId)
        {
            return ModelToDto.Parse(serviceRequestStatusRepository.Get(statusUpdateId));
        }
        public List<IServiceRequestStatusDto> GetAllStatuses()
        {
            return ModelToDto.Parse(serviceRequestStatusRepository.GetAll().ToList<IServiceRequestStatusModel>());
        }

        private bool LogError(IServiceRequestStatusDto srsDto)
        {
            var errorLogDto = new ErrorLogDto()
            {
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                ErrorSeverity = 1,
                ErrorSource = "ServiceRequestStatusUnitOfWork",
                ErrorMessage = "",
                MessageContent = "",
                LoggedTimestamp = LoggedTimestamp
        };
            ErrorLogRepository.Add(DtoToModel.Parse(errorLogDto));
            return false;
        }
        private bool LogError()
        {
            IServiceRequestStatusDto srhDto = new ServiceRequestStatusDto();
            return LogError(srhDto);
        }
        private bool LogError(List<IServiceRequestStatusDto> srsDtos)
        {
            bool result = true;
            srsDtos.ForEach(srsDto => result &= LogError(srsDto));
            return result;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}