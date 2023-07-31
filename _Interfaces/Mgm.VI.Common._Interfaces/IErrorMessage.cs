using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Common
{
    public interface IErrorMessage
    {
        string ErrorSource { get; set; }
        string ErrorText { get; set; }
        Exception ErrorException { get; set; }
        // IErrorMessage Instantiate(string errorSource, string errorText, Exception ex);
    }
}
