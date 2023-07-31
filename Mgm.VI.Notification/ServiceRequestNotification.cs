//////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Mgm.VI.Aws;
using Mgm.VI.Data.Dto;
using Mgm.VI.Data.Model;
using static Mgm.VI.Common.Constants;

namespace Mgm.VI.Notification
{
    public class ServiceRequestNotification : NotificationBase<IServiceRequestHistoryModel>, INotification<IServiceRequestModel>
    {
        public string ServiceRequestContent { get; set; }
        public ServiceRequestNotification() : base()
        { }
        public bool SendNotification()
        {
            var isSent = true;
            try
            {
                AwsSesSmtp ses = new AwsSesSmtp();
                ses.Body = ServiceRequestContent;
                if (IsContentSafe(ses.Body)) ses.SendMailSynch();
                else isSent = false;
            }
            catch (Exception e)
            {
                isSent = false;
                ErrorMessages.Add(string.Format("{0}::{1}", "ServiceRequestNotification", "SendNotification()"), e.Message, e);
                Logger.ChangeToApplicationLog().Log(e);
            }
            return isSent;
        }
    }
}