using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface ILineItemDto : IDto
    {
        #region Properties
        string ID { get; set; }
        // string AssetGroupId { get; set; }
        string ServicingStatus { get; set; }
        string IPMMedia { get; set; }
        string IPMTerritory { get; set; }
        string IPMLanguage { get; set; }
        string LicenseStart { get; set; }
        string LicenseEnd { get; set; }
        List<IOrderDto> Orders { get; set; }
        #endregion Properties
    }
}