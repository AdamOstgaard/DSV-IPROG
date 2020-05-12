using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace IPROG.Uppgifter.uppg2_2_1
{
    /// <summary>
    /// Generic class for sending small objects over UDP data-gram sockets.
    /// </summary>
    public class UdpSender: IDisposable
    {
        private readonly Socket _sendSocket;
        private readonly BinaryFormatter _serializer;

        /// <summary>
        /// Creates a new <see cref="UdpSender"/> instance.
        /// </summary>
        public UdpSender()
        {
            _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _serializer = new BinaryFormatter();
        }

        /// <summary>
        /// Setup address and port for the remote.
        /// </summary>
        /// <param name="address">Remote address</param>
        /// <param name="port">Remote port</param>
        public void Connect(IPAddress address, int port)
        {
            _sendSocket.Connect(address, port);
        }

        /// <summary>
        /// Serialize and send an object on the socket.
        /// </summary>
        /// <param name="data">object to send</param>
        public void Send(object data)
        {
            var bytes = GetBytes(data);

            if(bytes.Length <= 0)
            {
                return;
            }

            _sendSocket.Send(bytes);
        }

        /// <summary>
        /// Serialize data.
        /// </summary>
        /// <param name="data">The object to serialize</param>
        /// <returns>A byte array representing the serialized object.</returns>
        private byte[] GetBytes(object data)
        {
            using (var stream = new MemoryStream())
            {
                _serializer.Serialize(stream, data);

                return stream.ToArray();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Free all resources.
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
                _sendSocket.Dispose();
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
