using System;

namespace DEM.Engine
{
    public readonly struct Point2d
    {
        public static Point2d Zero = new Point2d(0, 0);

        public Point2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public float Distance(Point2d p2)
        {
            return (float)Math.Sqrt(Math.Pow(X - p2.X, 2) + Math.Pow(Y - p2.Y, 2));
        }

        public Vector2d Diff(Point2d v2)
        {
            return new Vector2d(X - v2.X, Y - v2.Y);
        }
    }
}