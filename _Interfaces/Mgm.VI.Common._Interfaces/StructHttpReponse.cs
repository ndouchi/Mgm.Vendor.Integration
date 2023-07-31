using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mgm.VI.Common
{
    public struct HttpResponseStruct
    {
        public string StatusCode;
        public string ReasonPhrase;
        public string ResponseMessage;
        public HttpResponseStruct(string statusCode, string reasonPhrase, string responseMessage)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            ResponseMessage = responseMessage;
        }
    }
}
