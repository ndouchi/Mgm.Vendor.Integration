//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Mgm.VI.Repository
{
    public interface IRepository<TEntity> : IDisposable
    {
        int Add(TEntity statusUpdateModel);
        TEntity Get(string id);
        IEnumerable<TEntity> GetAll();
        int Remove(string id);
        int Remove(int id);
        bool Save();
        int Update(TEntity statusUpdateModel);
    }
}