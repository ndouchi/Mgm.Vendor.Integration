using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public class LineItemModel : ILineItemModel
    {
        public string ID { get; set; }
        // public string AssetGroupId { get; set; }
        public string ServicingStatus { get; set; }
        public string IPMMedia { get; set; }
        public string IPMTerritory { get; set; }
        public string IPMLanguage { get; set; }
        public string LicenseStart { get; set; }
        public string LicenseEnd { get; set; }
        public List<IOrderModel> Orders { get; set; }
    }
}
