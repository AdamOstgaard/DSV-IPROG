using System;
using System.Drawing;

namespace IPROG.Uppgifter.uppg2_2_1
{
    /// <summary>
    /// Two connected points.
    /// </summary>
    [Serializable()]
    public struct PointSegment
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public PointSegment(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}
