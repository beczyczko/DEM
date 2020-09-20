using System.Numerics;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public class ParticleParticleCollisionSolver : CollisionSolver<Particle, Particle>
    {
        public override Vector2 CalculateCollisionForce(in Particle element1, in Particle element2)
        {
            if (element1.Equals(element2))
            {
                return Vector2.Zero;
            }
            else
            {
                // check collision, CollisionHappened method is not used to increase performance
                var positionDiff = element2.Position - element1.Position;

                var distance = positionDiff.Length();
                var radiusSum = element1.R + element2.R;

                if (distance > radiusSum)
                {
                    return Vector2.Zero;
                }
                else
                {
                    var dumpingFactor = DumpingFactor(element1, element2, positionDiff);

                    var deformation = (radiusSum - distance) / 2; // [m]

                    var equivalentSpringConstant = element1.K * element2.K / (element1.K + element2.K);

                    var bounceForce = -equivalentSpringConstant * dumpingFactor * deformation / distance; // N/m * m/m = N/m

                    // todo db need more analysis if force should be 2x bigger
                    // https://socratic.org/questions/what-is-the-spring-constant-in-parallel-connection-and-series-connection
                    return positionDiff * bounceForce;
                }
            }
        }

        public override bool CollisionHappened(in Particle element1, in Particle element2)
        {
            var positionDiff = element2.Position - element1.Position;

            var distance = positionDiff.Length();
            var radiusSum = element1.R + element2.R;

            return distance < radiusSum;
        }

        private float DumpingFactor(in Particle element1, in Particle element2, Vector2 positionDiff)
        {
            if (World.ParticlesBounceFactor == 1f)
            {
                return 1;
            }

            var currentVelocityDiff = element2.V - element1.V;

            var futurePositionDiff = positionDiff + currentVelocityDiff * 0.00001F; //todo db small value should work nice but better would be timeStep?

            var currentPositionDiffScalar = positionDiff.LengthSquared();
            var futurePositionDiffScalar = futurePositionDiff.LengthSquared();

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