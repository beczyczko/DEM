using System;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public class ParticleRigidWallCollisionSolver : CollisionSolver<Particle, RigidWall>
    {
        public override Vector2d CalculateCollisionForce(in Particle element1, in RigidWall element2)
        {
            if (CollisionHappened(element1, element2))
            {
                //todo db do not calculate the same things twice
                var closestPointOfWallToParticle = ClosestPointOfWallToParticle(element1, element2);
                var distanceFromParticleToWall = closestPointOfWallToParticle.Distance(element1.Position);

                var deltaX = closestPointOfWallToParticle.X - element1.Position.X; // [m]
                var deltaY = closestPointOfWallToParticle.Y - element1.Position.Y; // [m]

                if (distanceFromParticleToWall == 0)
                {
                    return Vector2d.Zero;
                }

                var deformation = element1.R - distanceFromParticleToWall; // [m]

                var dumpingFactor = DumpingFactor(element1, closestPointOfWallToParticle);
                var bounceForce = -element1.K * dumpingFactor * deformation / distanceFromParticleToWall; // N/m * m/m = N/m
                return new Vector2d(
                    bounceForce * deltaX, // N/m * m = N
                    bounceForce * deltaY
                    );
            }
            else
            {
                return Vector2d.Zero;
            }
        }

        public override bool CollisionHappened(in Particle element1, in RigidWall element2)
        {
            var closestPointOfWallToParticle = ClosestPointOfWallToParticle(element1, element2);
            var distanceFromParticleToWall = closestPointOfWallToParticle.Distance(element1.Position);

            return distanceFromParticleToWall < element1.R;
        }

        public Point2d ClosestPointOfWallToParticle(Particle particle, RigidWall rigidWall)
        {
            var A = rigidWall.P2.X - rigidWall.P1.X;
            var B = rigidWall.P2.Y - rigidWall.P1.Y;
            Point2d p3;

            var u = (float)((A * (particle.Position.X - rigidWall.P1.X) + B * (particle.Position.Y - rigidWall.P1.Y)) / (Math.Pow(A, 2) + Math.Pow(B, 2)));
            if (u <= 0)
            {
                p3 = rigidWall.P1;
            }
            else if (u >= 1)
            {
                p3 = rigidWall.P2;
            }
            else
            {
                p3 = new Point2d(rigidWall.P1.X + u * A, rigidWall.P1.Y + u * B);
            }

            return p3;
        }

        private float DumpingFactor(in Particle particle, in Point2d closestPointOfWallToParticle)
        {
            var currentPositionDiff = closestPointOfWallToParticle.Diff(particle.Position);

            var futurePositionDiff = currentPositionDiff.Add(particle.V.Multiply(-0.00001F)); //todo db small value should work nice but better would be timeStep?

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