using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface IServiceRequestStatusModel : IModel
    {
        #region Properties
        int Id { get; set; }
        string MasterDataName { get; set; }
        string MasterDataCode { get; set; }
        string MasterDataValue { get; set; }
        string SequenceOrder { get; set; }
        string CreatedBy { get; set; }
        string Comments { get; set; }
        bool Active { get; set; }
        string CreateDate { get; set; }
        string UpdateDate { get; set; }
        #endregion Properties
    }
}
