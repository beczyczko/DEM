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
                var deltaX = element2.Position.X - element1.Position.X; // [m]
                var deltaY = element2.Position.Y - element1.Position.Y; // [m]
                var distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY); // [m]
                var deformation = (element1.R + element2.R - distance) / 2; // [m]

                var equivalentSpringConstant = element1.K * element2.K / (element1.K + element2.K);

                var bounceForce = -equivalentSpringConstant * deformation / distance; // N/m * m/m = N/m

                // todo db need more analysis if force should be 2x bigger
                // https://socratic.org/questions/what-is-the-spring-constant-in-parallel-connection-and-series-connection
                return new Vector2d
                {
                    X = bounceForce * deltaX, // N/m * m = N
                    Y = bounceForce * deltaY
                };
            }
        }

        public override bool CollisionHappened(Particle element1, Particle element2)
        {
            var deltaX = element2.Position.X - element1.Position.X;
            var deltaY = element2.Position.Y - element1.Position.Y;

            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            var radiusSum = element1.R + element2.R;

            var wasHit = distance < radiusSum;
            return wasHit;
        }
    }
}