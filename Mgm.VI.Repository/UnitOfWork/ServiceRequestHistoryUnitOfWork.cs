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
    public class ServiceRequestHistoryUnitOfWork : UowBase, IDisposable
    {
        private bool disposed = false;
        private bool _rulesAreMet;
        private Storage storage;
        private ServiceRequestHistorySqlRepository serviceRequestHistoryRepository;
        private ErrorLogSqlRepository errorLogSqlRepository;

        private List<IServiceRequestHistoryDto> serviceRequestHistoryDtos;
        public ServiceRequestHistorySqlRepository ServiceRequestHistoryRepository
        {
            get
            {
                return this.serviceRequestHistoryRepository ?? new ServiceRequestHistorySqlRepository(storage.ConnectionString);
            }
        }
        public ErrorLogSqlRepository ErrorLogRepository
        {
            get
            {
                return this.errorLogSqlRepository ?? new ErrorLogSqlRepository(storage.ConnectionString);
            }
        }

        public List<IServiceRequestHistoryDto> ServiceRequestHistoryDtos
        {
            get
            {
                return this.serviceRequestHistoryDtos ?? new List<IServiceRequestHistoryDto>();
            }
            set
            {
                this.serviceRequestHistoryDtos = value;
            }
        }
        public List<IServiceRequestHistoryDto> ProcessedServiceRequestHistoryDtos { get; private set; }
        public List<IServiceRequestHistoryDto> DeadServiceRequestHistoryDtos { get; private set; }
        public string MessageBody { get; set; }
        public bool AllProcessed { get; set; }
        public ServiceRequestHistoryUnitOfWork(bool rulesAreMet,
                                        Storage store,
                                        List<IServiceRequestHistoryDto> srhDtos = null)
        {
            storage = store;
            _rulesAreMet = rulesAreMet;
            ServiceRequestHistoryDtos = srhDtos;
            ProcessedServiceRequestHistoryDtos = new List<IServiceRequestHistoryDto>();
            DeadServiceRequestHistoryDtos = new List<IServiceRequestHistoryDto>();
        }
        public IServiceRequestHistoryDto GetVendor(string statusUpdateId)
        {
            return ModelToDto.Parse(ServiceRequestHistoryRepository.Get(statusUpdateId));
        }
        public List<IServiceRequestHistoryDto> GetAllStatusUpdates()
        {
            return ModelToDto.Parse(ServiceRequestHistoryRepository.GetAll().ToList<IServiceRequestHistoryModel>());
        }
        public bool Save()
        {
            AllProcessed = _rulesAreMet ? SaveServiceRequestHistorys() : LogError();
            return AllProcessed;
        }
        private bool LogError(IServiceRequestHistoryDto srhDto)
        {
            var errorLogDto = new ErrorLogDto()
            {
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                ErrorSeverity = 1,
                ErrorSource = "ServiceRequestHistoryUnitOfWork",
                ErrorMessage = "",
                MessageContent = "",
                LoggedTimestamp = LoggedTimestamp
        };
            ErrorLogRepository.Add(DtoToModel.Parse(errorLogDto));
            return false;
        }
        private bool LogError()
        {
            IServiceRequestHistoryDto srhDto = new ServiceRequestHistoryDto();
            return LogError(srhDto);
        }
        private bool LogError(List<IServiceRequestHistoryDto> srhDtos)
        {
            bool result = true;
            srhDtos.ForEach(srhDto => result &= LogError(srhDto));
            return result;
        }
        private bool SaveServiceRequestHistorys()
        {
            bool allProcessed = true;
            if (ServiceRequestHistoryDtos != null && ServiceRequestHistoryDtos.Count > 0)
            {
                ServiceRequestHistoryDtos.ForEach(srhDto =>
                {
                    (ProcessedServiceRequestHistoryDtos ??= new List<IServiceRequestHistoryDto>()).Add(srhDto);
                    allProcessed &= (ServiceRequestHistoryRepository.Add(DtoToModel.Parse(srhDto)) > 0);
                });
            }
            else
                allProcessed = false;

            AllProcessed = allProcessed;
            return true;
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