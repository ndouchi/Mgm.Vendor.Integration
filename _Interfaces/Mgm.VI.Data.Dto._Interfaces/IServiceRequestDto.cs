using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto
{
    public interface IServiceRequestDto : IDto
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
        List<IContractDto> Contracts { get; set; }
        #endregion Properties
    }
}
