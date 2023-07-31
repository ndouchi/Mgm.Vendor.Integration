using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.IO
{
    public interface IFileNode
    {
        Guid NodeGuid { get; }
        string Name { get; }
        string PhysicalPath { get; }
        bool Exists { get; }
        bool Create(bool createDirectoryIfNotExisting = false);
        bool Delete();
    }
}
