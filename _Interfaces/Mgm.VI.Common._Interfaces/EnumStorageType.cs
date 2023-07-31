using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mgm.VI.Common
{
    public enum StorageType 
    {
        Database = 0,
        File = 1,
        ApplicationLog = 2, 
        ErrorLog = 3
    }
}
