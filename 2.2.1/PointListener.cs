using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg2_2_1
{
    /// <summary>
    /// A listener for point segments coming over UDP data-gram sockets.
    /// </summary>
    public class PointListener : IDisposable
    {
        private readonly BinaryFormatter _deserializer;
        private readonly Socket _socket;
        private Action<PointSegment> _callback;
        private bool _isRunning;

        /// <summary>
        /// Creates a new <see cref="PointListener"/> instance.
        /// </summary>
        public PointListener()
        {
            _callback = null;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _deserializer = new BinaryFormatter();
        }

        /// <summary>
        /// Set an action that should be called when a point is received.
        /// </summary>
        /// <param name="callback">An action taking a <see cref="PointSegment"/> as parameter.</param>
        public void SetCallback(Action<PointSegment> callback)
        {
            _callback = callback;
        }

        /// <summary>
        /// Start listen for incoming <see cref="PointSegment"/>.
        /// </summary>
        /// <param name="address">Interface to listen on.</param>
        /// <param name="port">Port to listen on.</param>
        public void StartListen(string address = "0.0.0.0", int port = 2000)
        {
            if (!IPAddress.TryParse(address, out var localAddress))
            {
                throw new ArgumentException("Address could not be parsed", nameof(address));
            }

            var endPoint = new IPEndPoint(localAddress, port);

            _socket.Bind(endPoint);
            _isRunning = true;

            Task.Factory.StartNew(ReceiveLoop, TaskCreationOptions.LongRunning);
        }
        
        /// <summary>
        /// Start reading data from the socket
        /// </summary>
        private void ReceiveLoop()
        {
            while (_isRunning) 
            {
                var bytesToRead = _socket.Available;

                if (bytesToRead <= 0)
                {
                    continue;
                }

                var buffer = new byte[bytesToRead];
                _socket.Receive(buffer, bytesToRead, SocketFlags.None);

                if (TryDeserializePointSegment(buffer, out var segment))
                {
                    _callback?.Invoke(segment);
                }
            }
        }

        /// <summary>
        /// Tries to deserialize the data into a <see cref="PointSegment"/>.
        /// </summary>
        /// <param name="bytes">The binary data to be deserialized.</param>
        /// <param name="result">The deserialized <see cref="PointSegment"/> if succeeded.</param>
        /// <returns>True if succeeded, false if unsuccessful.</returns>
        private bool TryDeserializePointSegment(byte[] bytes, out PointSegment result)
        {
            using (var stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var data = _deserializer.Deserialize(stream);

                if (data is PointSegment p)
                {
                    result = p;
                    return true;
                }
            }

            result = default;
            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Release all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            if (disposedValue)
            {
                return;
            }

            _isRunning = false;
            _socket?.Dispose();

            disposedValue = true;
        }
        #endregion
    }
}
