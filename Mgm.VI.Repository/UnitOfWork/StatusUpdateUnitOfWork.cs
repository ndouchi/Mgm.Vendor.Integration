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
   public class StatusUpdateUnitOfWork : UowBase, IDisposable
    {
        private bool disposed = false;
        private bool _rulesAreMet;
        private Storage storage;
        private StatusUpdateSqlRepository statusUpdateRepository;
        private ErrorLogSqlRepository errorLogSqlRepository;
        
        private List<IStatusUpdateDto> statusUpdateDtos; 
        public StatusUpdateSqlRepository StatusUpdateRepository
        {
            get
            {
                return this.statusUpdateRepository ?? new StatusUpdateSqlRepository(storage.ConnectionString);
            }
        }
        public ErrorLogSqlRepository ErrorLogRepository
        {
            get
            {
                return this.errorLogSqlRepository ?? new ErrorLogSqlRepository(storage.ConnectionString);
            }
        }

        public List<IStatusUpdateDto> StatusUpdateDtos
        {
            get
            {
                return this.statusUpdateDtos ?? new List<IStatusUpdateDto>();
            }
            set
            {
                this.statusUpdateDtos = value;
            }
        }
        public List<IStatusUpdateDto> ProcessedStatusUpdateDtos { get; private set; }
        public List<IStatusUpdateDto> DeadStatusUpdateDtos { get; private set; }
        public string MessageBody { get; set; }
        public bool AllProcessed { get; set; }

        public StatusUpdateUnitOfWork(bool rulesAreMet, 
                                        string messageBody,
                                        Storage store,
                                        List<IStatusUpdateDto> srDtos = null)
        {
            storage = store;
            _rulesAreMet = rulesAreMet;
            MessageBody = messageBody;
            StatusUpdateDtos = srDtos;
            ProcessedStatusUpdateDtos = new List<IStatusUpdateDto>();
            DeadStatusUpdateDtos = new List<IStatusUpdateDto>();
        }
        public IStatusUpdateDto GetVendor(string statusUpdateId)
        {
            return ModelToDto.Parse(StatusUpdateRepository.Get(statusUpdateId));
        }
        public List<IStatusUpdateDto> GetAllStatusUpdates()
        {
            return ModelToDto.Parse(StatusUpdateRepository.GetAll().ToList<IStatusUpdateModel>());
        }
        public bool Save()
        {
            AllProcessed = _rulesAreMet ? SaveStatusUpdates() : LogError();
            return AllProcessed;
        }
        private bool LogError(IStatusUpdateDto srDto)
        {
            var errorLogDto = new ErrorLogDto()
            {
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                ErrorSeverity = 1,
                ErrorSource = "StatusUpdateUnitOfWork",
                ErrorMessage = "",
                MessageContent = "",
                LoggedTimestamp = LoggedTimestamp
            };
            ErrorLogRepository.Add(DtoToModel.Parse(errorLogDto));
            return false;
        }
        private bool LogError()
        {
            IStatusUpdateDto srDto = new StatusUpdateDto(MessageBody);
            return LogError(srDto);
        }
        private bool LogError(List<IStatusUpdateDto> srDtos)
        {
            bool result = true;
            srDtos.ForEach(srDto => result &= LogError(srDto));
            return result;
        }
        private bool SaveStatusUpdates()
        {
            bool allProcessed = true;
            if (StatusUpdateDtos != null && StatusUpdateDtos.Count > 0)
            {
                StatusUpdateDtos.ForEach(srDto =>
                {
                    (ProcessedStatusUpdateDtos ??= new List<IStatusUpdateDto>()).Add(srDto);
                    allProcessed &= (StatusUpdateRepository.Add(DtoToModel.Parse(srDto)) > 0);
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