using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;

namespace Mgm.VI.Logger
{
    public class LoggerService : ILoggerService
    {
        private string _connectionString;
        private SystemName _systemName;
        private string _systemInfo;
        private EventType _eventType;
        private KeyValuePair<OptionalField, string>[] _optionalFieldValues; // this is reserved for special circumstances in cases of more elaborate logged messages
        private string _outMesage;
        LoggingMode _loggingMode = LoggingMode.ToDatabase;
        private string _logFilePath;

        public bool IsLoggedSuccessfully { get; set; }
        public string LogsDirectory { get; set; }
        public string FileName { get; set; }

        #region Constructors
        public LoggerService()
        {
            // These default configurations are for Office only.  Feel free to add another constructor that overrides
            // these configurations.

            _systemName = SystemName.MssVendorIntegration;
            _systemInfo = "MGM-Vendor Integration Layer";
            _eventType = EventType.Error;
        }
        public LoggerService(LoggingMode loggingMode, string logsDirectory) : this()
        {
            _loggingMode = loggingMode;
            LogsDirectory = logsDirectory;
            _logFilePath = Path.Combine(LogsDirectory, FileName);
            // CONSIDER a static instantiateion for : if (!File.Exists(_logFilePath))

        }
        #endregion Constructors

        #region Fluent ChangeTo... Logs
        public ILoggerService ChangeToDatabase()
        {
            _loggingMode = LoggingMode.ToDatabase;
            return this;
        }
        public ILoggerService ChangeToFile()
        {
            _loggingMode = LoggingMode.ToFile;
            return this;
        }
        public ILoggerService ChangeToDatabaseAndFile()
        {
            _loggingMode = LoggingMode.ToDatabaseAndFile;
            return this;
        }
        public ILoggerService ChangeToApplicationLog()
        {
            _loggingMode = LoggingMode.ToApplicationLog;
            return this;
        }
        public ILoggerService ChangeToAllLogs()
        {
            _loggingMode = LoggingMode.ToAllLogs;
            return this;
        }
        #endregion ChangeTo... Logs
        #region Sync Methods
        public bool Log(string message)
        {
            IsLoggedSuccessfully = false;
            switch (_loggingMode)
            {
                case LoggingMode.ToDatabase:
                    IsLoggedSuccessfully = LogToDatabase(message);
                    break;
                case LoggingMode.ToFile:
                    IsLoggedSuccessfully = LogToFile(message);
                    break;
                case LoggingMode.ToDatabaseAndFile:
                    IsLoggedSuccessfully = LogToDatabase(message);
                    break;
                case LoggingMode.ToApplicationLog:
                    IsLoggedSuccessfully = LogToApplicationLog(message);//TODO: Add Exception e as a parameter
                    break;
                case LoggingMode.ToAllLogs:
                    IsLoggedSuccessfully = LogToAllLogs(message);//TODO: Add Exception e as a parameter
                    break;
            }
            return IsLoggedSuccessfully;
        }

        private bool LogToAllLogs(string message)
        {
            throw new NotImplementedException();
        }
        #region Borrowed from IoUtility, due to Build Problems

        #region Private Methods
        private bool LogToDatabaseAndFile(string message)
        {
            var loggedToFile = LogToFile(message);
            var loggedToDb = LogToDatabase(message);
            return loggedToFile && loggedToDb;
        }
        private bool LogToFile(string message)
        {
            if (FileExists(_logFilePath)) //string.IsNullOrWhiteSpace(_logFilePath))
            {
                throw new Exception("_logFilePath is invalid or empty");
            }
            return LogToFile(_logFilePath, message);
        }
        private bool LogToDatabase(string message)
        {
            IsLoggedSuccessfully = false;

            return IsLoggedSuccessfully;
            //bool result;
            //Mgm.VI.Repository.
            //result = ErrorLogSqlRepository.Log(
            //                                     _systemName,
            //                                     _systemInfo,
            //                                     _eventType,
            //                                     message,                // This is the content of the logged message
            //                                     _connectionString,
            //                                     _optionalFieldValues,   //Nothing for the time being
            //                                     out _outMesage
            //                                  );
            //return result;
        }
        private bool LogToApplicationLog(string message, Exception e = null)
        {
            //var appLog = new EventLog("MGM-VU Application")
            //{
            //    Source = "Mgm.VI.Aws.Sqs.Processor"
            //};
            //appLog.WriteEntry(message);

            IsLoggedSuccessfully = false;
            //TODO: Change Implementation
            return false;
        }

        private bool FileExists(string logFilePath)
        {
            return File.Exists(logFilePath);
        }
        private bool LogToFile(string filePath, string message)
        {
            return WriteMessageToFile(filePath, message);
        }
        private bool WriteMessageToFile(string filePath, string message, bool append = true)
        {
            bool copyResult = true;
            if (!string.IsNullOrEmpty(filePath))
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                try
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    if (!File.Exists(filePath))
                    {
                        using (File.Create(filePath)) { }
                    }
                    using (StreamWriter sw = new StreamWriter(filePath, append))
                    { sw.WriteLine(message); }
                }
                catch (Exception ex)
                {
                    //TODO: Log Exception to the Application Even Log
                    copyResult = false;
                }
            }
            return copyResult;
        }
        #endregion Private Methods 
        #endregion Borrowed from IoUtility, due to Build Problems

        public static string GetLogFilePath(string rootPath, string logRootName, string logSubDirectory, string logName = "MasterLog")
        {
            rootPath = rootPath ?? "/";
            logRootName = logRootName ?? "!Logs";
            logSubDirectory = logSubDirectory ?? DateTime.Today.ToString("yyyy-MM-dd");

            return Path.Combine(rootPath, logRootName, logSubDirectory, $"{logName}.log");
        }
        public bool Log(Exception ex)
        {
            return Log(ex.Message);
        }
        public bool Log(string message, Exception ex)
        {
            return Log(String.Format("{0} : {1}", message, ex.Message));
        }
        #region NotImplementedException
        public bool Log(string message, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public bool Log(Exception ex, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public bool Log(string message, Exception ex, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public bool Log(string message, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }

        public bool Log(Exception ex, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }

        public bool Log(string message, EventType eventType, Exception ex, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }
        #endregion NotImplementedException
        #endregion Sync Methods

        #region Async Methods
        public void LogAsync(string message)
        {
            Task.Factory.StartNew(() => Log(message));
        }
        public void LogAsync(Exception ex)
        {
            Task.Factory.StartNew(() => Log(ex));
        }
        public void LogAsync(string message, Exception ex)
        {
            Task.Factory.StartNew(() => Log(message, ex));
        }
        public void LogAsync(string message, EventType eventType)
        {
            throw new NotImplementedException();
        }
        public void LogAsync(Exception ex, EventType eventType)
        {
            throw new NotImplementedException();
        }
        public void LogAsync(string message, Exception ex, EventType eventType)
        {
            throw new NotImplementedException();
        }
        public void LogAsync(string message, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }
        public void LogAsync(Exception ex, EventType eventType, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }
        public void LogAsync(string message, EventType eventType, Exception ex, KeyValuePair<OptionalField, string>[] optionalFieldValues)
        {
            throw new NotImplementedException();
        }
        #endregion Async Methods
    }
}