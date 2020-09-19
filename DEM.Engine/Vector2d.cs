using System;

namespace DEM.Engine
{
    public readonly struct Vector2d
    {
        public static Vector2d Zero => new Vector2d(0, 0);

        public Vector2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public Vector2d Add(Vector2d v2)
        {
            return new Vector2d(X + v2.X, Y + v2.Y);
        }

        public Vector2d Subtract(Vector2d v2)
        {
            return new Vector2d(X - v2.X, Y - v2.Y);
        }

        public Vector2d Multiply(float value)
        {
            return new Vector2d(X * value, Y * value);
        }

        public float Scalar => (float)Math.Sqrt(X * X + Y * Y);
    }
}