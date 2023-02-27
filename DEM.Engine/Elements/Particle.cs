using System;
using System.Numerics;

namespace DEM.Engine.Elements
{
    public struct Particle : ICollidable
    {
        public Particle(Vector2 position, float r, float m, float k, Vector2 v)
        {
            Position = position;
            R = r;
            M = m;
            K = k;
            V = v;
        }

        public Vector2 Position { get; set; }
        public float R { get; set; }
        public float M { get; set; }

        /// <summary>
        /// K - Spring Constant
        /// </summary>
        public float K { get; set; }
        public Vector2 V { get; set; }

        public void Move(float timeStep)
        {
            Position = new Vector2(Position.X + V.X * timeStep, Position.Y + V.Y * timeStep);
        }

        public Vector2 CalculateCollisionForce(ICollidable[] interactionElements)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, interactionElements);
        }

        public void ApplyForce(Vector2 force, float timeStep)
        {
            var dV = new Vector2(force.X * timeStep / M, force.Y * timeStep / M);
            V += dV;
        }

        public void ApplyGravityForce(in float gravity, float timeStep)
        {
            // m/s^2 * s = m/s
            var dV = new Vector2(0, gravity * timeStep);
            V += dV;
        }

        public Boundary Boundary => new Boundary(Position.Y - R, Position.Y + R, Position.X - R, Position.X + R);
    }

    public readonly struct Boundary
    {
        public Boundary(float top, float bottom, float left, float right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        public float Top { get; }
        public float Bottom { get; }
        public float Left { get; }
        public float Right { get; }

        public bool IsInside(Vector2 point)
        {
            return point.X >= Left
                   && point.X <= Right
                   && point.Y >= Top
                   && point.Y <= Bottom;
        }
    }
}