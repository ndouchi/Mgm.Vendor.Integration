using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Mgm.VI.Aws
{
    public interface IAwsSqsClient
    {
        string AccessKey { get; set; }
        string SecretKey { get; set; }
        RegionEndpoint RegionEndPoint { get; set; }
    }
}
