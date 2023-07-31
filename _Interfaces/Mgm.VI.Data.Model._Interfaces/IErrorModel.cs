using System;

namespace Mgm.VI.Data.Model
{
    public interface IErrorLogModel : IModel
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
