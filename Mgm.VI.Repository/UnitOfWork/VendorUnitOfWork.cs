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
    public class VendorUnitOfWork : UowBase, IDisposable
    {
        private bool disposed = false;
        private Storage storage;
        private VendorSqlRepository vendorRepository;
        private ErrorLogSqlRepository errorLogSqlRepository;

        private List<IVendorDto> vendorDtos;
        public VendorSqlRepository VendorRepository
        {
            get
            {
                return this.vendorRepository ?? new VendorSqlRepository(storage.ConnectionString);
            }
        }
        public ErrorLogSqlRepository ErrorLogRepository
        {
            get
            {
                return this.errorLogSqlRepository ?? new ErrorLogSqlRepository(storage.ConnectionString);
            }
        }

        public List<IVendorDto> VendorDtos
        {
            get
            {
                return this.vendorDtos ?? new List<IVendorDto>();
            }
            set
            {
                this.vendorDtos = value;
            }
        }
        public List<IVendorDto> ProcessedVendorDtos { get; private set; }
        public string MessageBody { get; set; }
        public bool AllProcessed { get; set; }
        public VendorUnitOfWork(Storage store, List<IVendorDto> vDtos = null)
        {
            storage = store;
            VendorDtos = vDtos;
            ProcessedVendorDtos = new List<IVendorDto>();
        }
        public bool Save()
        {
            AllProcessed = SaveVendors();
            return AllProcessed;
        }
        public int AddVendor(IVendorDto vendor)
        {
            return VendorRepository.Add(DtoToModel.Parse(vendor));
        }
        public int DeleteVendor(string vendorId)
        {
            return VendorRepository.Remove(vendorId);
        }
        public int UpdateVendor(IVendorDto vendor)
        {
            return VendorRepository.Update(DtoToModel.Parse(vendor));
        }
        public IVendorDto GetVendor(string vendorId)
        {
            return ModelToDto.Parse(VendorRepository.Get(vendorId));
        }
        public List<IVendorDto> GetAllVendors()
        {
            return ModelToDto.Parse(VendorRepository.GetAll().ToList<IVendorModel>());
        }
        private bool LogError(IVendorDto vDto)
        {
            var errorLogDto = new ErrorLogDto()
            {
                ApplicationName = ApplicationName,
                ApplicationPath = ApplicationPath,
                ErrorSeverity = 1,
                ErrorSource = "VendorUnitOfWork",
                ErrorMessage = "",
                MessageContent = "",
                LoggedTimestamp = LoggedTimestamp
            };
            ErrorLogRepository.Add(DtoToModel.Parse(errorLogDto));
            return false;
        }
        private bool LogError()
        {
            IVendorDto vDto = new VendorDto();
            return LogError(vDto);
        }
        private bool LogError(List<IVendorDto> vDtos)
        {
            bool result = true;
            vDtos.ForEach(vDto => result &= LogError(vDto));
            return result;
        }
        private bool SaveVendors()
        {
            bool allProcessed = true;
            if (VendorDtos != null && VendorDtos.Count > 0)
            {
                VendorDtos.ForEach(vDto =>
                {
                    (ProcessedVendorDtos ??= new List<IVendorDto>()).Add(vDto);
                    allProcessed &= (VendorRepository.Add(DtoToModel.Parse(vDto)) > 0);
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