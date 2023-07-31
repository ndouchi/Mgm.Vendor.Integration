using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Security
{
    public interface IUser
    {
        string CurrentUser { get; }
    }
}
