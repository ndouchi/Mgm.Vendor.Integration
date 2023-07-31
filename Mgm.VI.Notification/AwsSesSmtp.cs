using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Mgm.VI.Notification
{
    public class AwsSesSmtp : NotificationBase<AwsSesSmtp>
    {
        #region Properties
        public string HostServerName { get; set; }
        public int Port { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string AttachmentFile { get; set; }
        public string SentFrom { get; set; }
        public List<string> RecipientsList { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        #endregion Properties

        public AwsSesSmtp () { }
        public AwsSesSmtp (string hostServerName) 
        {
            HostServerName = hostServerName;
        }

        // Server Name:	email-smtp.us-west-2.amazonaws.com
        // Use Transport Layer Security(TLS):	Yes
        // <appSettings>
        //    <add key = "AWSAccessKey" value="Your AWS Access Key" />
        //    <add key = "AWSSecretKey" value="Your AWS secret Key" />
        // </appSettings>
        public void SendMailSynch()
        {
            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HostServerName);
            if (Port != 0) client.Port = Port;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(SentFrom);
            foreach (string recipient in RecipientsList)
            {
                mail.To.Add(recipient);
            }
            mail.Bcc.Add("ndouchi@mgm.com");
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            Attachment attachment1 = new Attachment(AttachmentFile, MediaTypeNames.Application.Octet);

            ContentDisposition disposition = attachment1.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(AttachmentFile);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(AttachmentFile);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(AttachmentFile);

            mail.Attachments.Add(attachment1);

            client.Send(mail);
        }
    }
}
