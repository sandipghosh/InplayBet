
namespace InplayBet.Web.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public class EmailSender
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EmailSender()
        {
            Attachment_ = null;
            Priority_ = MailPriority.Normal;
            Attachments = new List<Attachment>();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="Message">The body of the message</param>
        public void SendMail(string Message)
        {
            try
            {
                MailMessage message = new MailMessage();
                char[] Splitter = { ',', ';' };
                string[] AddressCollection = this.To.Split(Splitter);

                AddressCollection.Distinct().ToList().ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                        message.To.Add(x.Trim());
                });

                message.Subject = this.Subject;
                message.From = new MailAddress((this.From), (this.FromSenderName));
                message.Body = Message;
                message.Priority = this.Priority;
                message.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                message.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                message.IsBodyHtml = true;

                if (this.Attachments != null && this.Attachments.Count > 0)
                {
                    foreach (var item in this.Attachments)
                    {
                        message.Attachments.Add(item);
                    }
                }
                else if (this.Attachment != null)
                {
                    message.Attachments.Add(this.Attachment);
                }

                SmtpClient smtp = new SmtpClient(this.Server, this.Port);

                if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(this.UserName, this.Password);
                    smtp.EnableSsl = this.SSL;
                }

                smtp.Send(message);
                message.Dispose();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(Message);
                //throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Sends a piece of mail asynchronous
        /// </summary>
        /// <param name="Message">Message to be sent</param>
        public void SendMailAsync(string message, Action callback = null)
        {
            try
            {
                var asyncTestConn = Task.Factory.StartNew(() => SendMail(message), TaskCreationOptions.LongRunning);
                asyncTestConn.ContinueWith(task =>
                {
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            if (callback != null)
                                callback();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(message, callback);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whom the message is to
        /// </summary>
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// The subject of the email
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// Whom the message is from
        /// </summary>
        public string From
        {
            get
            {
                return string.IsNullOrEmpty(this.from) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_UID") : this.from;
            }
            set { from = value; }
        }

        /// <summary>
        /// Any attachments that are included with this
        /// message.
        /// </summary>
        public Attachment Attachment
        {
            get
            {
                return Attachment_;
            }
            set
            {
                Attachment_ = value;
            }
        }

        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// The priority of this message
        /// </summary>
        public MailPriority Priority
        {
            get { return Priority_; }
            set { Priority_ = value; }
        }

        /// <summary>
        /// Server Location
        /// </summary>
        public string Server
        {
            get
            {
                return string.IsNullOrEmpty(this.Server_) ?
                    CommonUtility.GetConfigData<string>("MAIL_SERVER") : this.Server_;
            }
            set { Server_ = value; }
        }

        /// <summary>
        /// User Name for the server
        /// </summary>
        public string UserName
        {
            get
            {
                return string.IsNullOrEmpty(this.UserName_) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_UID") : this.UserName_;
            }
            set { UserName_ = value; }
        }

        /// <summary>
        /// Password for the server
        /// </summary>
        public string Password
        {
            get
            {
                return string.IsNullOrEmpty(this.Password_) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_PWD") : this.Password_;
            }
            set { Password_ = value; }
        }

        /// <summary>
        /// Port to send the information on
        /// </summary>
        public int Port
        {
            get
            {
                return this.Port_ == 0 ?
                  CommonUtility.GetConfigData<int>("MAIL_SERVER_PORT") : this.Port_;
            }
            set { Port_ = value; }
        }

        /// <summary>
        /// Gets or sets the name of from sender.
        /// </summary>
        /// <value>
        /// The name of from sender.
        /// </value>
        public string FromSenderName
        {
            get
            {
                return string.IsNullOrEmpty(this.fromSenderName) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_FROM") : this.fromSenderName;
            }
            set
            {
                this.fromSenderName = value;
            }
        }

        public bool SSL
        {
            get
            {
                return CommonUtility.GetConfigData<bool>("MAIL_SERVER_SSL");
            }
        }
        #endregion

        #region Variables

        private MailPriority Priority_;
        private Attachment Attachment_;
        private string from;
        private string fromSenderName;
        private string subject;
        private string to;
        private string Server_;
        private string UserName_;
        private string Password_;
        private int Port_;
        private bool SSL_;
        #endregion
    }
}