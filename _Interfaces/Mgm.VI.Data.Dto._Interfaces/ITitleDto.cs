using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface ITitleDto : IDto
    {
        #region Properties
        string ID { get; set; }
        string Description { get; set; }
        string ServicingStatus { get; set; }
        string IPMStatus { get; set; }
        string ContractualDueDate { get; set; }
        string LicenseStartDate { get; set; }
        string EOPResource { get; set; }
        string PPSResource { get; set; }
        List<ILineItemDto> LineItems { get; set; }
        #endregion Properties
    }
}