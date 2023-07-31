using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public class ContractModel : IContractModel
    {
        public string ID { get; set; }
        public string ServicingStatus { get; set; }
        public string Description { get; set; }
        public List<ITitleModel> Titles { get; set; }

    }
}
