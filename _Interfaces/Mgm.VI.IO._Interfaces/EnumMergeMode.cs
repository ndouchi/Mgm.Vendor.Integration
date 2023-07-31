using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mgm.VI.IO
{
    public enum MergeMode
    {
        [Description("No Overwrite and No Merge")]
        NoOverwriteNoMerge = 0,
        [Description("Merge But No Overwrite")]
        MergeButNoOverwrite = 1,
        [Description("OverwriteAndMerge")]
        OverwriteAndMerge = 2,
    }
}
