using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPROG.Uppgifter.gesallprov.RemoteControl;
using IPROG.Uppgifter.gesallprov.ScreenSharing;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.gesallprov
{
    public partial class WatcherWindow : Form
    {
        private readonly TcpSerializer _client;
        private readonly MemoryStream _pictureStream;

        public WatcherWindow(IPAddress address, int port = 2000)
        {
            _pictureStream = new MemoryStream();
            _client = new TcpSerializer(address, port);
            InitializeComponent();
            SubscribeToUpdates();
        }

        /// <summary>
        /// Connect to the provided ScreenSharer and start listening for data.
        /// </summary>
        private void SubscribeToUpdates()
        {
            _client.Connect();
            _client.StartListening();
            _client.OnData += HandleOnData;
        }

        /// <summary>
        /// Determines if and how data received from the client should be handled
        /// </summary>
        /// <param name="sender">Source client</param>
        /// <param name="eventArgs">Object containing the received data</param>
        private void HandleOnData(object sender, OnReceivedDataEventArgs eventArgs)
        {
            // Check if the data is meant for us, otherwise return early.
            if (!(eventArgs.data is StreamedImage img))
            {
                return;
            }

            // update the image to show the SharingSession screen
            image.BeginInvoke(new Action(async () =>
                {
                    await UpdateImageAsync(img.data);
                    eventArgs.data = null;
                }
            ));
        }

        /// <summary>
        /// Sets the image asynchronously.
        /// </summary>
        /// <param name="imageData">image represented in bytes to be displayed.</param>
        private async Task UpdateImageAsync(byte[] imageData)
        {
            // clear previous image
            _pictureStream.SetLength(0);
            // write new image data to stream
            await _pictureStream.WriteAsync(imageData);
            // Assign new data stream to the image 
            image.Image = Image.FromStream(_pictureStream);
        }

        /// <summary>
        /// Handle mouse moves and send them to the remote computer
        /// </summary>
        private async void image_MouseMove(object sender, MouseEventArgs e)
        {
            await _client.SerializeAndSendAsync(new MouseRemoteMoveCommand{ Position = GetRemoteMousePosition()});
        }

        /// <summary>
        /// Gets and normalizes mouse's absolute position on the remote.
        /// </summary>
        /// <remarks>The image container is always covering the whole window but the image itself can be scaled by zooming to keep aspect ratio. This method converts the mouse coordinates relative to the container to coordinates relative to the actual itself.</remarks>
        /// <returns>A point representing the remote absolute position of the mouse</returns>
        private Point GetRemoteMousePosition()
        {
            if (image.Image == null)
            {
                return Point.Empty;
            }

            var originalPoint = image.PointToClient(Cursor.Position);

            // image and container dimensions
            var imageWidth = image.Image.Width;
            var imageHeight = image.Image.Height;
            var clientSizeWidth = image.ClientSize.Width;
            var clientSizeHeight = image.ClientSize.Height;

            var imageRatio = imageWidth / (float)imageHeight; // image W:H ratio
            var containerRatio = clientSizeWidth / (float)clientSizeHeight; // container W:H ratio

            // Check if image is higher or wider than the container.
            if (imageRatio >= containerRatio)
            {
                // horizontal image
                var scaleFactor = clientSizeWidth / (float)imageWidth;
                var scaledHeight = imageHeight * scaleFactor;
                // calculate gap between top of container and top of image
                var filler = Math.Abs(clientSizeHeight - scaledHeight) / 2;

                var x = (int)(originalPoint.X / scaleFactor);
                var y = (int)((originalPoint.Y - filler) / scaleFactor);

                return new Point(x, y);
            }
            else
            {
                // vertical image
                var scaleFactor = clientSizeHeight / (float)imageHeight;
                var scaledWidth = imageWidth * scaleFactor;
                var filler = Math.Abs(clientSizeWidth - scaledWidth) / 2;

                var x = (int)((originalPoint.X - filler) / scaleFactor);
                var y = (int)(originalPoint.Y / scaleFactor);

                return new Point(x, y);
            }
        }

        /// <summary>
        /// Handle and send mouse down to remote.
        /// </summary>
        private async void image_MouseDown(object sender, MouseEventArgs e)
        {
            await _client.SerializeAndSendAsync(new MouseRemoteClickCommand { Button = e.Button, Pressed = true });
        }

        /// <summary>
        /// Handle and send mouse up to remote.
        /// </summary>
        private async void image_MouseUp(object sender, MouseEventArgs e)
        {
            await _client.SerializeAndSendAsync(new MouseRemoteClickCommand { Button = e.Button, Pressed = false});
        }

        /// <summary>
        /// Handle and send keystrokes to the remote.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WatcherWindow_KeyDown(object sender, KeyEventArgs e)
        {
            await _client.SerializeAndSendAsync(new KeyDownRemoteCommand { Key = e.KeyCode });
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _client.OnData -= HandleOnData;
                _client.Dispose();
                _pictureStream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
