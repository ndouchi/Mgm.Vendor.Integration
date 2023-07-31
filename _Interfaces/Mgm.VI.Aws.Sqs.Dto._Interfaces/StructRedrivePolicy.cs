using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Aws.Sqs.Dto
{
    public struct RedrivePolicyStruct
    {
        int MaximumReceives;
        string DeadLetterQueue;
    }
}
