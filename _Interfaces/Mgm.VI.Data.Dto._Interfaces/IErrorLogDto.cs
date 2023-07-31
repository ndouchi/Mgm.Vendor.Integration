using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface IErrorLogDto : IDto
    {
        int LogId { get; set; }
        string ApplicationName { get; set; }
        string ApplicationPath { get; set; }
        short ErrorSeverity { get; set; }
        string ErrorSource { get; set; }
        string ErrorMessage { get; set; }
        string MessageContent { get; set; }
        string LoggedTimestamp { get; set; }
    }
}