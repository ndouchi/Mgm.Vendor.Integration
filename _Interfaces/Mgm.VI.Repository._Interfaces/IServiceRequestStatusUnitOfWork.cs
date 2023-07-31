//////////////////////////////////////////////////////////////////
using Mgm.VI.Data.Dto;
using System;
using System.Collections.Generic;

namespace Mgm.VI.Repository
{
    public interface IServiceRequestStatusUnitOfWork : IDisposable
    {
        IServiceRequestStatusDto GetStatus(string statusUpdateId);
        List<IServiceRequestStatusDto> GetAllStatuses();
    }
}