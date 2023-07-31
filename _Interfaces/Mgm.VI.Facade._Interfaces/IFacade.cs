using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgm.VI.Facade
{
    public interface IFacade
    {
        HttpResponseStruct HttpResponse { get; }
        IErrorMessages ErrorMessages { get; }
    }
}
