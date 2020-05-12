using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPROG.Uppgifter.uppg3_2_1
{
    public partial class Form1 : Form
    {
        private MailSender _sender;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logout by disposing the <see cref="MailSender"/> and enabling the login controls.
        /// </summary>
        private void Logout()
        {
            _sender.Dispose();
            _sender = null;
            EnableLogin();
        }

        /// <summary>
        /// Login to server and disable login controls.
        /// </summary>
        private async Task Login()
        {
            _sender?.Dispose();
            _sender = new MailSender(serverTextBox.Text, 465);

            try
            {
                await _sender.AuthenticateAndConnectAsync(usernameTextBox.Text, passwordTextBox.Text);

                DisableLogin();
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.ToString(), "Authentication Error!");
            }
        }

        /// <summary>
        /// Puts the application in logged out state
        /// </summary>
        private void EnableLogin()
        {
            mailInfoPanel.Enabled = false;
            bodyTextBox.Enabled = false;
            loginInputPanel.Enabled = true;

            Text = "Not connected";
            loginButton.Text = "Login";
        }

        /// <summary>
        /// Puts the application in logged in state
        /// </summary>
        private void DisableLogin()
        {
            bodyTextBox.Enabled = true;
            mailInfoPanel.Enabled = true;
            loginInputPanel.Enabled = false;

            Text = $"Connected and authenticated to {_sender.Host}:{_sender.Port}";
            loginButton.Text = "Logout";
        }

        /// <summary>
        /// Handle login button click.
        /// </summary>
        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (_sender != null && _sender.isAuthenticated)
            {
                Logout();
                return;
            }

            await Login();
        }

        /// <summary>
        /// Handle send mail button click. 
        /// </summary>
        private async void sendButton_Click(object sender, EventArgs e)
        {
            var msg = MailSender.CreateMessage(
                    fromTextBox.Text,
                    toTextBox.Text,
                    subjectTextBox.Text,
                    bodyTextBox.Text);

            try
            {
                await _sender.SendMessage(msg);

                MessageBox.Show("Email sent!");
            }
            catch (Exception err)
            {
                MessageBox.Show(this, err.ToString(), "Could not sen email!");
            }
        }
    }
}
