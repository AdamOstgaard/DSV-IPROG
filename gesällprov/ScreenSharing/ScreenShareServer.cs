using System.Net.Sockets;
using IPROG.Uppgifter.gesallprov.RemoteControl;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.gesallprov.ScreenSharing
{
    /// <summary>
    /// A <see cref="TcpServer"/> that Initializes a Screen recorder on Accept
    /// </summary>
    internal class ScreenShareServer : TcpServer
    {
        /// <summary>
        /// Gets or sets the recording instance for this server.
        /// </summary>
        public ScreenRecorder Recorder { get; private set; }

        /// <summary>
        /// Gets or sets the remote control subscription
        /// </summary>
        public RemoteControlCommandSubscription RemoteControl { get; private set; }

        /// <summary>
        /// Called when a new client connects. Wraps the socket in a <see cref="TcpSerializer"/> and attaches it to a <see cref="ScreenRecorder"/> and a <see cref="RemoteControlCommandSubscription"/>. 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected override TcpConnection Accept(Socket socket)
        {
            var client = new TcpSerializer(socket);

            Recorder = new ScreenRecorder(client);
            Recorder.StartRecording();

            RemoteControl = new RemoteControlCommandSubscription(client);
            client.StartListening();

            return client;
        }
    }
}
