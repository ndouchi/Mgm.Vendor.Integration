using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Logger
{
    public enum LoggingMode : int
    {
        ToDatabase = 0,
        ToFile = 1,
        ToDatabaseAndFile = 2, 
        ToApplicationLog = 3,
        ToAllLogs = 4
    }
}
