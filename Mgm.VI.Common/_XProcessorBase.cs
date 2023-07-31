using System.Xml.Linq;

namespace Mgm.VI.Common
{
    public class XProcessorBase : CommonBase
    {
        public XDocument XDoc { get; set; }

        public XProcessorBase(IErrorMessages errorMessages = null) : base(errorMessages) { }
    }
}