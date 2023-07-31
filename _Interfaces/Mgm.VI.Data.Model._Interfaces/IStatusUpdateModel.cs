using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public interface IStatusUpdateModel : IModel
    {
        #region Properties
        int StatusUpdateId { get; set; }
        string VendorId { get; set; }
        string VendorName { get; set; }
        string ServiceRequestId { get; set; }
        string Comments { get; set; }
        string MessageContent { get; set; }
        string SqsRetrievalTimestamp { get; set; }
        bool IsPersistedToMss { get; set; }
        #endregion Properties
    }
}
