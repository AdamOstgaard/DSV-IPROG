using System.Net.Sockets;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.uppg2_1_2
{
    /// <summary>
    /// Simple chat server for broadcasting and relaying messages.
    /// </summary>
    internal class ChatServer : TcpServer
    {
        private readonly ClientHandler _clientHandler;

        /// <summary>
        /// Creates a new <see cref="ChatServer"/> instance.
        /// </summary>
        public ChatServer()
        {
            _clientHandler = new ClientHandler();
        }

        /// <summary>
        /// Starts accepting new connections.
        /// </summary>
        /// <param name="address">Listening interface default: 0.0.0.0 (all)</param>
        /// <param name="port">Listening port default: 2000</param>
        public new void StartAccept(string address = "0.0.0.0", int port = 2000)
        {
            base.StartAccept(address, port);

            // Update window title.
            ConsoleTitleBuilder.IP = LocalAddress.ToString();
            ConsoleTitleBuilder.Port = port;
        }

        /// <summary>
        /// Accepts the newly connected client and add it to the attached <see cref="ClientHandler"/>.
        /// </summary>
        /// <param name="socket">Socket of the new connection.</param>
        /// <returns>The accepted client.</returns>
        protected override TcpConnection Accept(Socket socket)
        {
            var client = new ChatServerClient(socket);
            client.StartListening();

            _clientHandler.AddClient(client);

            return client;
        }
    }
}
