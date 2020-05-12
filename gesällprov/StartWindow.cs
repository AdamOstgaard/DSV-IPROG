using System;
using System.Net;
using System.Windows.Forms;
using IPROG.Uppgifter.gesallprov.ScreenSharing;
using Timer = System.Threading.Timer;

namespace IPROG.Uppgifter.gesallprov
{
    public partial class StartWindow : Form
    {
        private ScreenRecorder Recorder => _screenShareServer.Recorder;
        private ScreenShareServer _screenShareServer;
        private Timer _fpsUpdateTimer;

        public StartWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts the required components for a screen sharing session.
        /// </summary>
        private void StartScreenCapture()
        {
            _screenShareServer = new ScreenShareServer();
            _screenShareServer.StartAccept();

            StartFpsCounterUpdater();
        }

        /// <summary>
        /// Starts a timer that is called ever 100ms to update the Fps counter.
        /// </summary>
        private void StartFpsCounterUpdater()
        {
            _fpsUpdateTimer = new Timer(_ =>
                    UpdateFpsCounter(
                        (int)(Recorder?.CurrentFrameRate ?? 0)),
                null,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100));
        }

        /// <summary>
        /// Sets the fps counter
        /// </summary>
        /// <param name="fps">Fps to be displayed</param>
        private void UpdateFpsCounter(int fps) 
        {
            // Check if we are on another thread than UI-thread.
            if (FrameRateCounter.InvokeRequired)
            {
                FrameRateCounter.Invoke(new Action(() => FrameRateCounter.Text = "fps: " + fps));
                return;
            }

            FrameRateCounter.Text = "fps: " + fps;
        }

        /// <summary>
        /// Start watching session
        /// </summary>
        private void WatchButtonClick(object sender, EventArgs e)
        {
            var ipStr = serverTextBox.Text;

            if (IPAddress.TryParse(ipStr, out var ipAddress))
            {
                new WatcherWindow(ipAddress).Show();
                return;
            }

            MessageBox.Show("Invalid ip address");
        } 
        

        /// <summary>
        /// Start sharing session.
        /// </summary>
        private void ShareButton_Click(object sender, EventArgs e)
            => StartScreenCapture();
        

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _screenShareServer?.Dispose();
                _fpsUpdateTimer?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
