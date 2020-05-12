using System.Drawing;
using System.Windows.Forms;

namespace IPROG.Uppgifter.uppg2_2_1
{
    public partial class WhiteBoard : Form
    {
        private bool _isDrawing;
        private Point _previousMousePosition;

        private readonly Pen _pen;
        private readonly Graphics _graphics;
        private readonly UdpSender _udpSender;

        /// <summary>
        /// Start the application
        /// </summary>
        /// <param name="udpSender">The remote program</param>
        /// <param name="pointListener">Receiver</param>
        public WhiteBoard(UdpSender udpSender, PointListener pointListener)
        {
            InitializeComponent();
            _graphics = canvas.CreateGraphics();
            _pen = new Pen(Color.Black);
            _previousMousePosition = new Point();
            _udpSender = udpSender;

            pointListener.SetCallback(DrawLine);
        }

        /// <summary>
        /// Handle mouse down
        /// </summary>
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
        }

        /// <summary>
        /// Handle local drawing
        /// </summary>
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && !_previousMousePosition.IsEmpty)
            {
                var segment = new PointSegment(_previousMousePosition, e.Location);

                DrawLine(segment);
                _udpSender.Send(segment);
            }

            _previousMousePosition = e.Location;
        }

        /// <summary>
        /// Handle mouse up
        /// </summary>
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }
        
        /// <summary>
        /// Draw line on canvas.
        /// </summary>
        private void DrawLine(PointSegment segment)
        {
            _graphics.DrawLine(_pen, segment.Point1, segment.Point2);
        }
    }
}
