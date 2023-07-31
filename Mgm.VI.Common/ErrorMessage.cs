using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Common
{
    public class ErrorMessage : IErrorMessage
    {
        public string ErrorSource { get; set; }
        public string ErrorText { get; set; }
        public Exception ErrorException { get; set; }
        public ErrorMessage(string errorSource, string errorText, Exception ex = null) {
            this.ErrorSource = errorSource;
            this.ErrorText = errorText;
            ErrorException ??= ex;
        }
    }
}
