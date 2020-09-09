using System;

namespace DEM.Engine
{
    public struct Point2d
    {
        public static Point2d Zero = new Point2d(0, 0);

        public Point2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float Distance(Point2d p2)
        {
            return (float)Math.Sqrt(Math.Pow(X - p2.X, 2) + Math.Pow(Y - p2.Y, 2));
        }
    }
}