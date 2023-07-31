using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Mgm.VI.Common;
using System.Xml.Schema;

namespace Mgm.VI.Data.Dto 
{
    public class ErrorLogDto : DtoBase, IErrorLogDto
    {
        public static readonly string XmlFieldName = "ErrorLog";
        public static readonly string XmlGroupingFieldName = "ErrorLogs";

        #region Properties
        public int LogId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public short ErrorSeverity { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageContent { get; set; }
        public string LoggedTimestamp { get; set; }
        #endregion Properties

        private void Initialize(int logId, string applicationName,
                                string applicationPath, short errorSeverity,
                                string errorSource, string errorMessage,
                                string messageContent, string loggedTimestamp)
        {
            try
            {
                #region Initialize Properties
                LogId = logId;
                ApplicationName = applicationName;
                ApplicationPath = applicationPath;
                ErrorSeverity = errorSeverity;
                ErrorSource = errorSource;
                ErrorMessage = errorMessage;
                MessageContent = messageContent;
                LoggedTimestamp = loggedTimestamp;
                #endregion Initialize Properties
                IsHydrated = true;
            }
            catch (Exception ex)
            {
                IsHydrated = false;
                ErrorMessages.Add("ErrorLogDto::Initialize(...)", ex.Message, ex);
            }
        }

        public void Initialize(XElement xElement, IErrorMessages errorMessages = null)
        {
            throw new NotImplementedException();
        }
        #region Constructors
        public ErrorLogDto() : base() { }
        public ErrorLogDto(int logId, string applicationName,
                                string applicationPath, short errorSeverity,
                                string errorSource, string errorMessage,
                                string messageContent, string loggedTimestamp) : this()
        {
            Initialize( logId, applicationName, applicationPath, 
                        errorSeverity, errorSource,
                        errorMessage, messageContent, loggedTimestamp);
        }
        #endregion Constructors

        #region Static Parsers
        #endregion Static Parsers
    }
}
