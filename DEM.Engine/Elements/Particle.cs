﻿namespace DEM.Engine.Elements
{
    public struct Particle : ICollidable
    {
        public Particle(Point2d position, float r, float m, float k, Vector2d v)
        {
            Position = position;
            R = r;
            M = m;
            K = k;
            V = v;
        }

        public Point2d Position { get; set; }
        public float R { get; set; }
        public float M { get; set; }

        /// <summary>
        /// K - Spring Constant
        /// </summary>
        public float K { get; set; }
        public Vector2d V { get; set; }

        public void Move(float timeStep)
        {
            Position = new Point2d(Position.X + V.X * timeStep, Position.Y + V.Y * timeStep);
        }

        public Vector2d CalculateCollisionForce(ICollidable[] interactionElements)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, interactionElements);
        }

        public void ApplyForce(Vector2d force, float timeStep)
        {
            var dV = new Vector2d(force.X * timeStep / M, force.Y * timeStep / M);
            V = V.Add(dV);
        }

        public void ApplyGravityForce(in float gravity, float timeStep)
        {
            // m/s^2 * s = m/s
            var dV = new Vector2d(0, gravity * timeStep);
            V = V.Add(dV);
        }
    }
}