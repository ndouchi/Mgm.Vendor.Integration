
using Mgm.VI.Common;
using System.Xml.Linq;

namespace Mgm.VI.Facade
{
    public class FacadeBase
    {
        protected string __vendorUserId;
        protected string __vendorId;
        protected Storage storage;
        public HttpResponseStruct HttpResponse { get; private set; }
        public IErrorMessages ErrorMessages { get; private set; }
        public FacadeBase(string vendorUserId, string vendorId, IErrorMessages errorMessages = null)
        {
            Initialize(vendorUserId, vendorId, errorMessages);
        }
        private void Initialize(string vendorUserId, string vendorId, IErrorMessages errorMessages = null)
        {
            __vendorUserId = vendorUserId;
            __vendorId = vendorId;
            ErrorMessages = errorMessages ?? new ErrorMessages();
        }
    }
}
