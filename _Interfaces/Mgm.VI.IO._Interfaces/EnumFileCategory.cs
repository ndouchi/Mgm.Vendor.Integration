using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mgm.VI.IO
{
    public enum FileCategory
    {
        [Description("All and Any Type of File")]
        All = 0,
        [Description("Media")]
        Media = 1,
        [Description("Document")]
        Documents = 2,
    }
}
