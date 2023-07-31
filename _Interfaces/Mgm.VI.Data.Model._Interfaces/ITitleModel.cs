using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface ITitleModel : IModel
    {
        string ID { get; set; }
        string Description { get; set; }
        string ServicingStatus { get; set; }
        string IPMStatus { get; set; }
        string ContractualDueDate { get; set; }
        string LicenseStartDate { get; set; }
        string EOPResource { get; set; }
        string PPSResource { get; set; }
        List<ILineItemModel> LineItems { get; set; }
    }
}