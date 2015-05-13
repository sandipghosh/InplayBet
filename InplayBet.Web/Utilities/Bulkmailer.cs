

namespace InplayBet.Web.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;

    public class Bulkmailer
    {
        #region Private variables
        private const int clientcount = 7;
        private SmtpClient[] smtpClients = new SmtpClient[clientcount + 1];
        private CancellationTokenSource cancelToken;

        private MailPriority priority;
        private string from;
        private string fromSenderName;
        private string subject;
        private string to;
        private string server;
        private string username;
        private string password;
        private int port;
        private bool ssl;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// Gets or sets to list.
        /// </summary>
        /// <value>
        /// To list.
        /// </value>
        public List<string> ToList { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
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
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public MailPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server
        {
            get
            {
                return string.IsNullOrEmpty(this.server) ?
                    CommonUtility.GetConfigData<string>("MAIL_SERVER") : this.server;
            }
            set { server = value; }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get
            {
                return string.IsNullOrEmpty(this.username) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_UID") : this.username;
            }
            set { username = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                return string.IsNullOrEmpty(this.password) ?
                    CommonUtility.GetConfigData<string>("MAIL_SENDER_PWD") : this.password;
            }
            set { password = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port
        {
            get
            {
                return this.port == 0 ?
                  CommonUtility.GetConfigData<int>("MAIL_SERVER_PORT") : this.port;
            }
            set { port = value; }
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="Bulkmailer"/> is SSL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SSL; otherwise, <c>false</c>.
        /// </value>
        public bool SSL
        {
            get
            {
                return CommonUtility.GetConfigData<bool>("MAIL_SERVER_SSL");
            }
        }
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="Bulkmailer"/> class.
        /// </summary>
        public Bulkmailer()
        {
            priority = MailPriority.Normal;
            SetupSMTPClients();
        }

        /// <summary>
        /// Starts the email run.
        /// </summary>
        /// <param name="content">The content.</param>
        public void StartEmailRun(string content)
        {
            try
            {
                ParallelOptions parallelOptions = new ParallelOptions();
                //Create a cancellation token so you can cancel the task.
                cancelToken = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancelToken.Token;
                //Manage the MaxDegreeOfParallelism instead of .NET Managing this. We dont need 500 threads spawning for this.
                parallelOptions.MaxDegreeOfParallelism = System.Environment.ProcessorCount * 10;
                try
                {
                    Parallel.ForEach(this.ToList, parallelOptions, (string toAddress) =>
                    {
                        try
                        {
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress((this.From), (this.FromSenderName));
                            message.To.Add(toAddress);
                            message.Subject = this.Subject;
                            message.Body = content;
                            message.Priority = this.Priority;
                            message.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                            message.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                            message.IsBodyHtml = true;
                            SendEmail(message);
                        }
                        catch (Exception ex1)
                        {
                            //ex1.ExceptionValueTracker();
                        }
                    });
                }
                catch (OperationCanceledException ex2)
                {
                    //User has cancelled this request.
                    //ex2.ExceptionValueTracker();
                }
            }
            finally
            {
                DisposeSMTPClients();
            }
        }

        /// <summary>
        /// Cancels the email run.
        /// </summary>
        public void CancelEmailRun()
        {
            cancelToken.Cancel();
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void SendEmail(MailMessage msg)
        {
            try
            {
                bool gotlocked = false;
                while (!gotlocked)
                {
                    //Keep looping through all smtp client connections until one becomes available
                    for (int i = 0; i <= clientcount; i++)
                    {
                        if (System.Threading.Monitor.TryEnter(smtpClients[i]))
                        {
                            try
                            {
                                smtpClients[i].Send(msg);
                            }
                            finally
                            {
                                System.Threading.Monitor.Exit(smtpClients[i]);
                            }
                            gotlocked = true;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    //Do this to make sure CPU doesn't ramp up to 100%
                    System.Threading.Thread.Sleep(1);
                }
            }
            finally
            {
                msg.Dispose();
            }
        }

        /// <summary>
        /// Setups the SMTP clients.
        /// </summary>
        private void SetupSMTPClients()
        {
            for (int i = 0; i <= clientcount; i++)
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient(this.Server, this.Port);
                    if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.Credentials = new System.Net.NetworkCredential(this.UserName, this.Password);
                        smtpClient.EnableSsl = this.SSL;
                    }
                    smtpClients[i] = smtpClient;
                }
                catch (Exception ex)
                {
                    ex.ExceptionValueTracker();
                }
            }
        }

        /// <summary>
        /// Disposes the SMTP clients.
        /// </summary>
        private void DisposeSMTPClients()
        {
            for (int i = 0; i <= clientcount; i++)
            {
                try
                {
                    smtpClients[i].Dispose();
                }
                catch (Exception ex)
                {
                    ex.ExceptionValueTracker();
                }
            }
        }
    }
}