using Mgm.VI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mgm.VI.Services
{
    public interface IService
    {
        HttpResponseStruct HttpResponse { get; }
        IErrorMessages ErrorMessages { get; }
    }
}
