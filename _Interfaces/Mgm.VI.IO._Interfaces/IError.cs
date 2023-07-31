using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.IO
{
    public interface IError
    {
        bool IsAnException { get; }
        string Description { get; set; }
        Exception ExceptionRaised { get; }
    }
}
