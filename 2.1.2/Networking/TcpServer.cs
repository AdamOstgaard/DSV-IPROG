using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.Networking
{
    /// <summary>
    /// A simple <see cref="TcpServer"/> for accepting and handling <see cref="TcpConnection"/>.
    /// </summary>
    public abstract class TcpServer : IDisposable
    {
        /// <summary>
        /// Address of the local interface which the server listens on.
        /// </summary>
        public IPAddress LocalAddress => _localAddress;

        private IPAddress _localAddress;

        private readonly Socket _socket;
        private readonly List<TcpConnection> _clients;

        /// <summary>
        /// Creates a new TcpServer instance.
        /// </summary>
        protected TcpServer()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<TcpConnection>();
        }

        /// <summary>
        /// Starts listening for incoming connections.
        /// </summary>
        /// <param name="address">Interface to listen on</param>
        /// <param name="port">Port to listen on</param>
        public void StartAccept(string address = "0.0.0.0", int port = 2000)
        {
            if (!IPAddress.TryParse(address, out _localAddress))
            {
                throw new ArgumentException("Address could not be parsed", nameof(address));
            }

            var endPoint = new IPEndPoint(LocalAddress, port);

            _socket.Bind(endPoint);
            _socket.Listen(10);

            Task.Factory.StartNew(AcceptLoop, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Accepts incoming connections.
        /// </summary>
        private async Task AcceptLoop()
        {
            while (true)
            {
                var clientSocket = await _socket.AcceptAsync();

                _clients.Add( Accept(clientSocket));
            }
        }

        /// <summary>
        /// Override this method to determine what to do with the newly connected socket. This is called when a new client connects.
        /// </summary>
        /// <param name="socket">Client socket</param>
        /// <returns>A <see cref="TcpConnection"/> handling the socket.</returns>
        protected abstract TcpConnection Accept(Socket socket);

        /// <summary>
        /// Free up used resources and dispose all connected clients.
        /// </summary>
        public void Dispose()
        {
            _socket?.Dispose();
            _clients.ForEach(c => c.Dispose());
        }
    }
}
