using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface IContractDto : IDto
    {
        #region Properties
        string ID { get; set; }
        string ServicingStatus { get; set; }
        string Description { get; set; }
        List<ITitleDto> Titles { get; set; }
        #endregion Properties
    }
}