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
    public class AwsSns
    {
        // IAM User Name: ses-smtp.MgmVu
        string _hostServerName = "email-smtp.us-west-2.amazonaws.com";
        int _port = 25;// Port:	25, 465 or 587
        string _smtpUsername = "AKIARCNE5WDCUU5UHDVH";
        string _smtpPassword = "BPrqUBycrzv3EYOIzMvV4aD58630EmN0nAw4231uApWi";
        string _attachmentFile = "BPrqUBycrzv3EYOIzMvV4aD58630EmN0nAw4231uApWi";
        string _sentFrom = "";
        List<string> _recipientsList = new List<string>();
        string _subject = "";
        string _body = "";

        public string HostServerName
        {
            get
            {
                return _hostServerName;
            }
            set
            {
                _hostServerName = value;
            }
        }
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }
        public string SmtpUsername
        {
            get
            {
                return _smtpUsername;
            }
            set
            {
                _smtpUsername = value;
            }
        }
        public string SmtpPassword
        {
            get
            {
                return _smtpPassword;
            }
            set
            {
                _smtpPassword = value;
            }
        }
        public string AttachmentFile
        {
            get
            {
                return _attachmentFile;
            }
            set
            {
                _attachmentFile = value;
            }
        }
        public string SentFrom
        {
            get
            {
                return _sentFrom;
            }
            set
            {
                _sentFrom = value;
            }
        }
        public List<string> RecipientsList
        {
            get
            {
                return _recipientsList;
            }
            set
            {
                _recipientsList = value;
            }
        }
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
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
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_hostServerName);
            if (Port != 0) client.Port = Port;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(_sentFrom);
            foreach (string recipient in _recipientsList)
            {
                mail.To.Add(recipient);
            }
            mail.Bcc.Add("ndouchi@mgm.com");
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;

            Attachment attachment1 = new Attachment(_attachmentFile, MediaTypeNames.Application.Octet);


            ContentDisposition disposition = attachment1.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(_attachmentFile);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(_attachmentFile);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(_attachmentFile);

            mail.Attachments.Add(attachment1);

            client.Send(mail);
        }
    }
}
