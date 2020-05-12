using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.Networking
{
    public abstract class TcpConnection : IDisposable
    {
        /// <summary>
        /// Remote Hostname of the connected client.
        /// </summary>
        public string HostName => _socket.RemoteEndPoint.ToString();

        private NetworkStream _networkStream;
        private Stream _syncStream;

        private readonly Socket _socket;
        private readonly IPEndPoint _endPoint;

        /// <summary>
        /// Gets a synchronized <see cref="NetworkStream"/> of the connected client.
        /// </summary>
        protected Stream Stream
        {
            get
            {
                if (NetworkStream != null && _syncStream == null)
                {
                    // create synchronized stream to allow for async/concurrent access.
                    _syncStream = Stream.Synchronized(_networkStream);
                }

                return _syncStream;
            }
        }

        /// <summary>
        /// Gets the raw network stream of the connected client.
        /// </summary>
        private NetworkStream NetworkStream
        {
            get
            {
                // make sure there is only one instance of the stream.
                if (_networkStream == null && _socket.Connected)
                {
                    _networkStream = new NetworkStream(_socket);
                }

                return _networkStream;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TcpConnection"/> with the provided endpoint information
        /// </summary>
        /// <param name="ipAddress">Remote address of the connection.</param>
        /// <param name="port">Remote port of the connection.</param>
        protected TcpConnection(IPAddress ipAddress, int port) : this(new IPEndPoint(ipAddress, port))
        {
        }

        /// <summary>
        /// Creates a new <see cref="TcpConnection"/> with the provided endpoint information
        /// </summary>
        /// <param name="endpoint">Remote endpoint of the connection.</param>
        protected TcpConnection(IPEndPoint endpoint) 
        {
            _endPoint = endpoint;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Creates a new <see cref="TcpConnection"/> from the provided socket.
        /// </summary>
        /// <param name="socket">Socket to use for connection.</param>
        /// <exception cref="ArgumentNullException">If the socket or its remote address is null</exception>
        protected TcpConnection(Socket socket) 
        {
            if (!(socket?.RemoteEndPoint is IPEndPoint))
            {
                throw new ArgumentNullException(nameof(socket));
            }

            _socket = socket;
        }

        /// <summary>
        /// Open the connection synchronously.
        /// </summary>
        public void Connect()
        {
            Task.Run(ConnectAsync).Wait();
        }

        /// <summary>
        /// Open the connection asynchronously.
        /// </summary>
        public async Task ConnectAsync()
        {
            if (!_socket.Connected)
            {
                await _socket.ConnectAsync(_endPoint);
            }
        }

        /// <summary>
        /// Starts listening for data on the connection.
        /// </summary>
        public void StartListening()
        {
            if (!IsSocketOpen())
            {
                throw new Exception("Socket is not open.");
            }

            Task.Factory.StartNew(ReceiveLoop, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Sends the provided bytes over the connection.
        /// </summary>
        /// <param name="bytes">Bytes to send</param>
        public async Task SendBytesAsync(byte[] bytes)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            try
            {
                // always start by sending the length of the message first.
                var msgLength = BitConverter.GetBytes(bytes.Length);

                Array.Resize(ref msgLength, bytes.Length + msgLength.Length);
                Array.Copy(bytes,0 , msgLength, 4, bytes.Length);

                await Stream.WriteAsync(msgLength, 0, msgLength.Length);
            }
            catch (SocketException)
            {
                SocketDisconnected();

                Dispose();
            }
        }

        /// <summary>
        /// Listens for data on the connected socket, reads it and calls <see cref="ReceivedBytes"/> when data is read.
        /// </summary>
        private async Task ReceiveLoop()
        {
            while (_socket.Connected)
            {
                var msgLength = await ReadMessageLength();
                var msg = await ReadMessage(msgLength);

                ReceivedBytes(msg);
            }

            SocketDisconnected();
            Dispose();
        }

        /// <summary>
        /// Reads a message from the stream and returns it as a byte array.
        /// </summary>
        /// <param name="msgLength">Length of the message</param>
        /// <returns>A byte array containing the data read from the stream.</returns>
        private async Task<byte[]> ReadMessage(int msgLength)
        {
            var msgBuffer = new byte[msgLength];

            var read = 0;

            do
            {
                read += await Stream.ReadAsync(msgBuffer, read, msgLength - read);
            } while (read < msgLength);

            return msgBuffer;
        }

        /// <summary>
        /// Reads the first four bytes of the stream to get the length of the next message.
        /// </summary>
        /// <returns>Length in number of bytes of next message.</returns>
        private async Task<int> ReadMessageLength()
        {
            if (!_socket.Connected)
            {
                throw new ObjectDisposedException(nameof(_socket), "Socket is not open!");
            }

            var buffer = await ReadMessage(4);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Returns true if the socket is open or false if it is closed.
        /// </summary>
        /// <returns>True if the socket is open or false if it is closed.</returns>
        public bool IsSocketOpen()
            => !_socket.Poll(1000, SelectMode.SelectRead) || _socket.Available > 0;

        /// <summary>
        /// Called when the socket disconnects.
        /// </summary>
        protected abstract void SocketDisconnected();

        /// <summary>
        /// Called when new data was read
        /// </summary>
        /// <param name="bytes"></param>
        protected abstract void ReceivedBytes(byte[] bytes);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _syncStream?.Dispose();
                _socket?.Dispose();
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
