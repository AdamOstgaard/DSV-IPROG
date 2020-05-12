using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace IPROG.Uppgifter.uppg3_2_1
{
    /// <summary>
    /// Class for doing simple smtp operations using MailKit.
    /// </summary>
    public class MailSender : IDisposable
    {
        public string Host { get; }
        public int Port { get; }
        public bool isAuthenticated => _smtpClient.IsAuthenticated;

        private readonly SmtpClient _smtpClient;

        /// <summary>
        /// Creates a new <see cref="MailSender"/> instance.
        /// </summary>
        /// <param name="host">Hostname for outgoing mail.</param>
        /// <param name="port">Port for outgoing mail</param>
        public MailSender(string host, int port)
        {
            Host = host;
            Port = port;
            _smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Send a message asynchronously.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public async Task SendMessage(MimeMessage message)
        {
            if (!_smtpClient.IsAuthenticated)
            {
                throw new ServiceNotAuthenticatedException("Smtp client is not authenticated.");
            }

            await _smtpClient.SendAsync(message);
        }

        /// <summary>
        /// Connect and login to the remote SMTP server.
        /// </summary>
        /// <param name="username">Username to authenticate with</param>
        /// <param name="password">Password to authenticate with</param>
        /// <returns></returns>
        public async Task AuthenticateAndConnectAsync(string username, string password)
        {
            await _smtpClient.ConnectAsync(Host, Port);

            var credentials = new System.Net.NetworkCredential(username, password);

            await _smtpClient.AuthenticateAsync(credentials);
        }

        /// <summary>
        /// Creates a simple MailMessage
        /// </summary>
        /// <param name="from">Mail from</param>
        /// <param name="to">Mail recipient</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail body</param>
        /// <exception cref="ArgumentException">Thrown if address could not be parsed.</exception>
        /// <returns>The created message.</returns>
        public static MimeMessage CreateMessage(string from, string to, string subject, string body)
        {
            if(!InternetAddress.TryParse(to, out var toAddress))
            {
                throw new ArgumentException("Could not parse recipient address.", nameof(to));
            }

            if (!InternetAddress.TryParse(from, out var fromAddress))
            {
                throw new ArgumentException("Could not parse sender address.", nameof(from));
            }

            return new MimeMessage(
                from: new[] { fromAddress }, 
                to: new[] { toAddress }, 
                subject: subject, 
                body: new TextPart(MimeKit.Text.TextFormat.Plain) {
                    Text = body
                });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Release used resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _smtpClient.Disconnect(quit: true);
                _smtpClient.Dispose();
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
