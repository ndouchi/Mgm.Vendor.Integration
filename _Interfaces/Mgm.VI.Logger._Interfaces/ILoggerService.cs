using System;
using System.Collections.Generic;
using System.Text;

namespace Mgm.VI.Logger
{
    public interface ILoggerService
    {
        ILoggerService ChangeToDatabase();
        ILoggerService ChangeToFile();
        ILoggerService ChangeToDatabaseAndFile();
        ILoggerService ChangeToApplicationLog();
        ILoggerService ChangeToAllLogs();

        #region Sync Methods
        bool Log(string message);
        bool Log(Exception ex);
        bool Log(string message, Exception ex);

        bool Log(string message, EventType eventType);
        bool Log(Exception ex, EventType eventType);
        bool Log(string message, Exception ex, EventType eventType);

        bool Log(string message, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        bool Log(Exception ex, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        bool Log(string message, EventType eventType, Exception ex, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        #endregion Sync Methods

        #region Async Methods
        void LogAsync(string message);
        void LogAsync(Exception ex);
        void LogAsync(string message, Exception ex);

        void LogAsync(string message, EventType eventType);
        void LogAsync(Exception ex, EventType eventType);
        void LogAsync(string message, Exception ex, EventType eventType);

        void LogAsync(string message, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        void LogAsync(Exception ex, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        void LogAsync(string message, EventType eventType, Exception ex, KeyValuePair<OptionalField, string>[] optionalFieldValues);
        #endregion Async Methods
    }
}