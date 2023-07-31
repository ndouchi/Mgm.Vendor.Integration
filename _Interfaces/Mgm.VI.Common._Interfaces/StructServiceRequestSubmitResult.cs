using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mgm.VI.Common
{
    public struct ServiceRequestSubmitResult 
    {
        public string ServiceRequestSubmitId { get; set; }
        public string ErrorMessage { get; set; }
        public ServiceRequestSubmitResult(string serviceRequestSubmitId, string errorMessage)
        {
            ServiceRequestSubmitId = serviceRequestSubmitId;
            ErrorMessage = errorMessage;
        }
    }
}
