
using System;

namespace Mgm.VI.Data.Model
{
    public class RejectionModel : IRejectionModel
    {
        public RejectionStatusCodeEnum StatusCode { get; set; }
        public string ID { get; set; }
        public string CurrentStatus { get; set; }
        public string Asset { get; set; }
        public string RejectionCode { get; set; }
        public string Issue { get; set; }
        public string CommentsHistory { get; set; }
        public string RejectedBy { get; set; }
        public string RejectionDate { get; set; }
        public string Urgency { get; set; }
        public string RootCause { get; set; }
        public string Document { get; set; }
    }
}
