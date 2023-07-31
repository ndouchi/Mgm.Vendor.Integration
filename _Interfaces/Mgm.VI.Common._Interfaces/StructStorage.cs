using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mgm.VI.Common
{
    public struct Storage 
    {
        public StorageType Type;
        public string Directory;
        public string ConnectionString;
        public Storage(StorageType type, string directory, string connectionString)
        {
            Type = type;
            Directory = directory;
            ConnectionString = connectionString;
        }
    }
}
