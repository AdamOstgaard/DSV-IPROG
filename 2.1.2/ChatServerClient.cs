using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg2_1_2
{
    internal delegate void OnClientDisconnectedEventHandler(ChatServerClient sender, OnClientDisconnectedEventArgs e);
    internal delegate void OnMessageEventHandler(ChatServerClient sender, OnMessageEventArgs e);

    internal class ChatServerClient : Networking.TcpConnection
    {
        public event OnClientDisconnectedEventHandler OnDisconnect;
        public event OnMessageEventHandler OnMessage;

        public ChatServerClient(Socket socket) : base(socket)
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
        /// Called when the client disconnects and notifies any event listeners of the change.
        /// </summary>
        protected override void SocketDisconnected()
            => OnDisconnect?.Invoke(this, new OnClientDisconnectedEventArgs { Client = HostName });

        /// <summary>
        /// Called when new data has been read and notifies any event listeners with the message.
        /// </summary>
        /// <param name="bytes">Incoming bytes</param>
        protected override void ReceivedBytes(byte[] bytes)
        {
            var text = Encoding.UTF8.GetString(bytes);

            OnMessage?.Invoke(this, new OnMessageEventArgs
            {
                Sender = HostName,
                MessageText = text
            });
        }
    }
}
