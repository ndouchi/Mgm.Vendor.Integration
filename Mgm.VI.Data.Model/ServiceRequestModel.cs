
using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public class ServiceRequestModel : IServiceRequestModel
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string ServicingStatus { get; set; }
        public string RushOrder { get; set; }
        public string DueDate { get; set; }
        public string BusinessPartnerID { get; set; }
        public string BusinessPartner { get; set; }
        public string ProfileID { get; set; }
        public string ProfileDescription { get; set; }
        public string FastTrack { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CompletedDate { get; set; }
        public List<IContractModel> Contracts { get; set; }
    }
}
