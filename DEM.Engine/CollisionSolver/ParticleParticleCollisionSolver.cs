using System;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public class ParticleParticleCollisionSolver : CollisionSolver<Particle, Particle>
    {
        public override Vector2d CalculateCollisionForce(Particle element1, Particle element2)
        {
            if (element1.Equals(element2) || !CollisionHappened(element1, element2))
            {
                return new Vector2d();
            }
            else
            {
                const float springFactor = -1F; // [N/m] //todo db how to handle different particles

                var deltaX = element2.X - element1.X; // [m]
                var deltaY = element2.Y - element1.Y; // [m]
                var distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY); // [m]
                var deformation = (element1.R + element2.R - distance) / 2; // [m]
                var bounceForce = springFactor * deformation / distance; // N/m * m/m = N/m
                return new Vector2d
                {
                    X = bounceForce * deltaX, // N/m * m = N
                    Y = bounceForce * deltaY
                };
            }
        }

        public override bool CollisionHappened(Particle element1, Particle element2)
        {
            var deltaX = element2.X - element1.X;
            var deltaY = element2.Y - element1.Y;

            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            var radiusSum = element1.R + element2.R;

            var wasHit = distance < radiusSum;
            return wasHit;
        }
    }
}