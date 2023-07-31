using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public class TitleModel : ITitleModel
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public string ServicingStatus { get; set; }
        public string IPMStatus { get; set; }
        public string ContractualDueDate { get; set; }
        public string LicenseStartDate { get; set; }
        public string EOPResource { get; set; }
        public string PPSResource { get; set; }
        public List<ILineItemModel> LineItems { get; set; }
    }
}
