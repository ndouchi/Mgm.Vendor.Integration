using Amazon;
using System;

namespace Mgm.VI.Aws
{
    public class AwsSqsClient : IAwsSqsClient
    {
        public AwsSqsClient() { }
        public AwsSqsClient(string accessKey, string secretKey, RegionEndpoint regionEndPoint)
        {
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
            this.RegionEndPoint = regionEndPoint;
        }
        public AwsSqsClient(IAwsUser awsUser, RegionEndpoint regionEndPoint) 
        {
            this.AccessKey = awsUser.AccessKey;
            this.SecretKey = awsUser.SecretKey;
            this.RegionEndPoint = regionEndPoint;
        }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public RegionEndpoint RegionEndPoint { get; set; }
    }
}
