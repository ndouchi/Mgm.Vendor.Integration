using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.IO
{
    public interface IDirectoryNode
    {
        Guid NodeGuid { get; }
        bool IsLeaf { get; }
        bool IsEmpty { get; } // Is Empty of both files and directories
        bool IsEmptyOfFiles { get; }
        string Name { get; }
        string PhysicalPath { get; }
        bool Exists { get; }
        IDirectoryNode[] Subdirectories { get; set; }
        IFileNode[] Files { get; set; }
        bool Create(); // Physically create this directory in the files system
        bool Delete(bool recursive = true); // Delete this directory.  If recursive is set to true, then delete 
        bool DeleteFiles();// Delete the top-level files in this directory
        bool DeleteSubdirectories();// Delete the top-level subdirectories in this directory

    }
}
