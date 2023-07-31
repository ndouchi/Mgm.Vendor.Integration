
using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface IServiceRequestHistoryDto : IDto
    {
        int Id { get; set; }
        string VendorId { get; set; }
        string VendorName { get; set; }
        string ServiceRequestId { get; set; }
        string Comments { get; set; }
        string MessageContent { get; set; }
        string SubmissionTimestamp { get; set; }
    }
}