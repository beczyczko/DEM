using System;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public class ParticleParticleCollisionSolver : CollisionSolver<Particle, Particle>
    {
        public override Vector2d CalculateCollisionForce(in Particle element1, in Particle element2)
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

                var dumpingFactor = DumpingFactor(element1, element2);
                var bounceForce = -equivalentSpringConstant * dumpingFactor * deformation / distance; // N/m * m/m = N/m

                // todo db need more analysis if force should be 2x bigger
                // https://socratic.org/questions/what-is-the-spring-constant-in-parallel-connection-and-series-connection
                return new Vector2d(
                    bounceForce * deltaX, // N/m * m = N
                    bounceForce * deltaY
                );
            }
        }

        public override bool CollisionHappened(in Particle element1, in Particle element2)
        {
            var deltaX = element2.Position.X - element1.Position.X;
            var deltaY = element2.Position.Y - element1.Position.Y;

            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            var radiusSum = element1.R + element2.R;

            var wasHit = distance < radiusSum;
            return wasHit;
        }

        private float DumpingFactor(in Particle element1, in Particle element2)
        {
            var currentPositionDiff = element2.Position.Diff(element1.Position);
            var currentVelocityDiff = element2.V.Subtract(element1.V);

            var futurePositionDiff = currentPositionDiff.Add(currentVelocityDiff.Multiply(0.00001F)); //todo db small value should work nice but better would be timeStep?

            var currentPositionDiffScalar = currentPositionDiff.Scalar;
            var futurePositionDiffScalar = futurePositionDiff.Scalar;

            if (currentPositionDiffScalar < futurePositionDiffScalar) // dismissal
            {
                return 1 * World.ParticlesBounceFactor;
            }
            else // approach
            {
                return 1 / World.ParticlesBounceFactor;
            }
        }
    }
}