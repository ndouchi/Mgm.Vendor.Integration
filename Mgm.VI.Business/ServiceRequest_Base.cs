using Mgm.VI.Common;

namespace Mgm.VI.Business
{
    public abstract class ServiceRequest_Base
    {
        public IErrorMessages Errors { get; private set; }
        public string XsdFilePath { get; set; }
        public ServiceRequest_Base() { }
    }
}