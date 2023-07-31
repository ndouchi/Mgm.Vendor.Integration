namespace Mgm.VI.Data.Dto
{
    public interface IRejectionDto : IDto
    {
        #region Properties
        RejectionStatusCodeEnum StatusCode { get; set; }

        string ID { get; set; }
        string CurrentStatus { get; set; }
        string Asset { get; set; }
        string RejectionCode { get; set; }
        string Issue { get; set; }
        string CommentsHistory { get; set; }
        string RejectedBy { get; set; }
        string RejectionDate { get; set; }
        string Urgency { get; set; }
        string RootCause { get; set; }
        string Document { get; set; }
        #endregion Properties
    }
}