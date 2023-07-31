using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface IContractModel : IModel
    {
        string ID { get; set; }
        string ServicingStatus { get; set; }
        string Description { get; set; }
        List<ITitleModel> Titles { get; set; }
    }
}