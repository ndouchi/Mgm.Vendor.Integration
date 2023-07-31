using System;

namespace Mgm.VI.Aws
{
    public interface IAwsUser
    {
            string AccessKey { get; set; }
            string SecretKey { get; set; }
    }
}
