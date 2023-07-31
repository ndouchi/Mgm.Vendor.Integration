//////////////////////////////////////////////////////////////////
using Mgm.VI.Aws;
using Mgm.VI.Common;
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Mgm.VI.Common.Constants.Db;
using Mgm.VI.Logger;

namespace Mgm.VI.Notification
{
    public abstract class NotificationBase<T> where T : class
    {
        protected ILoggerService Logger { get; set; }
        public IErrorMessages ErrorMessages { get; set; }

        public NotificationBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            ErrorMessages = new ErrorMessages();
            Logger = new LoggerService();
        }

        protected bool IsContentSafe(string content)
        {
            return true; // Add rules
        }
        protected bool Send()
        {
            bool sendResult = false;
            try
            {
                sendResult = true;
            }
            catch (Exception e)
            {

                ErrorMessages.Add(String.Format("NotificationBase::Send()"), e.Message, e);
            }
            finally
            {
            }
            return sendResult;
        }

    }
}