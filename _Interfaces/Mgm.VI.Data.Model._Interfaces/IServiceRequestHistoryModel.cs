using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface IServiceRequestHistoryModel : IModel
    {
        #region Properties
        int Id { get; set; }
        string ServiceRequestId { get; set; }
        string VendorId { get; set; }
        string VendorName { get; set; }
        string Comments { get; set; }
        string MessageContent { get; set; }
        string SubmissionTimestamp { get; set; }
        #endregion Properties
    }
}
