using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg2_1_1
{
    /// <summary>
    /// Event handler for new messages received by the client.
    /// </summary>
    /// <param name="sender">Source object</param>
    /// <param name="e">Object containing information about the received message</param>
    internal delegate void OnMessageEventHandler(object sender, OnMessageEventArgs e);

    /// <summary>
    /// Chat client for sending and receiving simple messages
    /// </summary>
    internal class ChatClient : Networking.TcpConnection
    {
        public event OnMessageEventHandler OnMessage;

        /// <summary>
        /// Creates a new <see cref="ChatClient"/> instance.
        /// </summary>
        /// <param name="ipAddress">Ip address for the server to connect to</param>
        /// <param name="port">Port of the server to connect to</param>
        public ChatClient(IPAddress ipAddress, int port) : base(ipAddress, port)
        {
        }

        /// <summary>
        /// Send a text message to the server asynchronously.
        /// </summary>
        /// <param name="message">Text string to be sent</param>
        public async Task SendMessageAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            await SendBytesAsync(bytes);
        }

        /// <summary>
        /// Called when connection is lost to notify any event listeners of the disconnection.
        /// </summary>
        protected override void SocketDisconnected()
        {
            OnMessage?.Invoke(this, new OnMessageEventArgs
            {
                MessageText = "Lost connection to server!"
            });
        }

        /// <summary>
        /// Called when new data has been read and notifies any event listeners with the message
        /// </summary>
        /// <param name="bytes">Incoming bytes</param>
        protected override void ReceivedBytes(byte[] bytes)
        {
            var text = Encoding.UTF8.GetString(bytes);

            OnMessage?.Invoke(this, new OnMessageEventArgs
            {
                MessageText = text
            });
        }
    }
}