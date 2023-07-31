using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface IServiceRequestModel : IModel
    {
        #region Properties
        string ID { get; set; }
        string Description { get; set; }
        string TransactionType { get; set; }
        string ServicingStatus { get; set; }
        string RushOrder { get; set; }
        string DueDate { get; set; }
        string BusinessPartnerID { get; set; }
        string BusinessPartner { get; set; }
        string ProfileID { get; set; }
        string ProfileDescription { get; set; }
        string FastTrack { get; set; }
        string CreatedDate { get; set; }
        string CreatedBy { get; set; }
        string CompletedDate { get; set; }
        List<IContractModel> Contracts { get; set; }
        #endregion Properties
    }
}
