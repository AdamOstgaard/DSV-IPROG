using System;
using System.Windows.Forms;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.gesallprov.RemoteControl
{
    internal class RemoteControlCommandSubscription : IDisposable
    {
        private readonly TcpSerializer _connection;

        /// <summary>
        /// Initializes a new <see cref="RemoteControlCommandSubscription"/> and subscribes to messages coming from the provided connection
        /// </summary>
        /// <param name="connection">Connection to listen for messages on.</param>
        public RemoteControlCommandSubscription(TcpSerializer connection)
        {
            _connection = connection;
            connection.OnData += HandleOnData;
        }

        /// <summary>
        /// Handles the data based on type and calls the corresponding Method.
        /// </summary>
        /// <param name="sender">The object invoking the event</param>
        /// <param name="e">The data received</param>
        private static void HandleOnData(object sender, OnReceivedDataEventArgs e)
        {
            switch (e.data)
            {
                case MouseRemoteMoveCommand c:
                    HandleRemoteMouseMoveEvent(c);
                    break;
                case MouseRemoteClickCommand c:
                    HandleRemoteMouseButtonEvent(c);
                    break;
                case KeyDownRemoteCommand c:
                    HandleRemoteKeyDownEvent(c);
                    break;

            }
        }

        /// <summary>
        /// Move the mouse to the provided position
        /// </summary>
        /// <param name="c">Mouse move data</param>
        private static void HandleRemoteMouseMoveEvent(MouseRemoteMoveCommand c)
        {
            Cursor.Position = c.Position;
        }

        /// <summary>
        /// Check if and how the mouse event should be handled
        /// </summary>
        /// <param name="c">Mouse click data</param>
        private static void HandleRemoteMouseButtonEvent(MouseRemoteClickCommand c)
        {
            switch (c.Button)
            {
                case MouseButtons.Left when c.Pressed:
                    MouseInterop.LeftButtonDown();
                    break;
                case MouseButtons.Left:
                    MouseInterop.LeftButtonUp();
                    break;
                case MouseButtons.Right when c.Pressed:
                    MouseInterop.RightButtonDown();
                    break;
                case MouseButtons.Right:
                    MouseInterop.RightButtonUp();
                    break;
            }
        }

        /// <summary>
        /// Sends the key to the focused application.
        /// </summary>
        /// <remarks>This currently only works with letter keys (A-Za-z)</remarks>
        /// <param name="c">Key press data</param>
        private static void HandleRemoteKeyDownEvent(KeyDownRemoteCommand c)
        {
            SendKeys.SendWait(c.Key.ToString());
        }

        /// <summary>
        /// Disposable support
        /// </summary>
        public void Dispose()
        {
            _connection.OnData -= HandleOnData;
        }
    }
}
