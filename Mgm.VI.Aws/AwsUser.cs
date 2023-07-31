using System;

namespace Mgm.VI.Aws
{
    public class AwsUser : IAwsUser
    {
        public AwsUser() { }
        public AwsUser(string accessKey, string secretKey) 
        {
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
        }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
