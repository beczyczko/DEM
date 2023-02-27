using System.Numerics;

namespace DEM.Engine.Elements
{
    public struct Particle : ICollidable
    {
        public Particle(Vector2 position, Vector2 positionOld, float r, float m, float k)
        {
            Position = position;
            PositionOld = positionOld;
            R = r;
            M = m;
            K = k;
            A = Vector2.Zero;
        }

        public Vector2 Position { get; set; }
        public Vector2 PositionOld { get; set; }
        public float R { get; set; }
        public float M { get; set; }
        public Vector2 A { get; set; }

        /// <summary>
        /// K - Spring Constant
        /// </summary>
        public float K { get; set; }

        public Vector2 V => Position - PositionOld;

        /// <summary>
        /// Ek - Kinetic energy
        /// </summary>
        public float Ek => M * V.Length() * V.Length() / 2;

        public void Move(float timeStep)
        {
            //todo db other Integration Methods
            var v = V;
            PositionOld = Position;
            Position = Position + v + A * timeStep * timeStep; // Verlet Integration
            A = Vector2.Zero;
        }

        public Vector2 CalculateCollisionForce(ICollidable[] interactionElements)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, interactionElements);
        }

        public void ApplyForce(Vector2 force, float timeStep)
        {
            A += force * timeStep / M;
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