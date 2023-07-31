using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mgm.VI.IO
{
    public enum CopyMode
    {
        [Description("Recursively, copy the entire tree, every file in every sub-directory")]
        Recursive = 0,
        [Description("Shallow, first level files, no directories.")]
        ShallowFilesOnly = 1,
        [Description("Shallow, first level files, create first level sub-directories without copying their content.")]
        ShallowFirstLevelFilesAndSubdirectoryNamesOnly = 2,
        [Description("Shallow, first level files, copy first level sub-directories with their file-content only.")]
        ShallowFirstLevelFilesAndSubdirectories = 3,
    }
}
