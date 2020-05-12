using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.gesallprov.ScreenSharing
{
    /// <summary>
    /// Records screen content and writes it to a <see cref="TcpSerializer"/>.
    /// </summary>
    public class ScreenRecorder
    {
        /// <summary>
        /// Currently captured frame rate in Frames/Second
        /// </summary>
        public double CurrentFrameRate => TimeSpan.FromSeconds(1).TotalMilliseconds / _lastFrameTime.TotalMilliseconds;
        
        /// <summary>
        /// Gets or sets the maximum frame rate to record at
        /// </summary>
        public int TargetFrameRate { get; set; } = 30;

        private TimeSpan TargetFrameTime => TimeSpan.FromSeconds(1) / TargetFrameRate;

        private TimeSpan _lastFrameTime;
        private bool _isRunning;

        private readonly TcpSerializer _client;

        /// <summary>
        /// Creates a new <see cref="ScreenRecorder"/> instance.
        /// </summary>
        /// <param name="client">Target to write frames to</param>
        public ScreenRecorder(TcpSerializer client)
        {
            _client = client;
            _isRunning = false;
        }

        /// <summary>
        /// Creates and starts a new recording.
        /// </summary>
        public void StartRecording()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            Task.Factory.StartNew(async () =>
            {
                var s = new Stopwatch();
                while (_isRunning)
                {
                    s.Restart();
                    var img = CaptureScreen();
                    await WriteToTarget(img);
                    _lastFrameTime = s.Elapsed;

                    // Sleep if frame rate exceeds max target frame rate.
                    Thread.Sleep(TargetFrameTime < _lastFrameTime ? TimeSpan.Zero : TargetFrameTime - _lastFrameTime);
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public void StopRecording() => _isRunning = false;

        /// <summary>
        /// Creates a bitmap from the screen
        /// </summary>
        /// <remarks>Only screen with index 0 will be captured.</remarks>
        /// <returns>A screen shot of the Screen0 </returns>
        private static Bitmap CaptureScreen()
        {
            // Select bounds of first screen
            var bounds = Screen.AllScreens[0].Bounds;
            var captureBitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            var captureGraphics = Graphics.FromImage(captureBitmap);

            // Create screen shot
            captureGraphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);

            return captureBitmap;
        }

        /// <summary>
        /// Writes a image to the client.
        /// </summary>
        /// <param name="image">Image to be written</param>
        /// <returns></returns>
        private async Task WriteToTarget(Image image)
        {
            var (codec, parameters) = ImageEncoder.GetImageEncoder(ImageFormat.Jpeg, 20L);
            await using var memoryStream = new MemoryStream();

            image.Save(memoryStream, codec, parameters);

            await _client.SerializeAndSendAsync(new StreamedImage { data = memoryStream.GetBuffer() });
        }
    }
}
