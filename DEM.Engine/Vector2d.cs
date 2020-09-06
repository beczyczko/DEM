﻿namespace DEM.Engine
{
    public struct Vector2d
    {
        public static Vector2d Zero => new Vector2d(0, 0);

        public Vector2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public Vector2d Add(Vector2d v2)
        {
            return new Vector2d(X + v2.X, Y + v2.Y);
        }
    }
}