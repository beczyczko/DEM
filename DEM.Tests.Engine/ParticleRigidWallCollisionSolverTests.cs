using System.Collections.Generic;
using DEM.Engine;
using DEM.Engine.CollisionSolver;
using DEM.Engine.Elements;
using FluentAssertions;
using Xunit;

namespace DEM.Tests.Engine
{
    public class ParticleRigidWallCollisionSolverTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ParticleWallCollisionTests(
            Particle particle,
            RigidWall rigidWall,
            Point2d expectedClosestPointOfWall,
            Vector2d expectedCollisionForce)
        {
            var particleRigidWallCollisionSolver = new ParticleRigidWallCollisionSolver();
            var closestPointOfWallToParticle = particleRigidWallCollisionSolver.ClosestPointOfWallToParticle(particle, rigidWall);
            closestPointOfWallToParticle.Should().Be(expectedClosestPointOfWall);
            var calculateCollisionForce = particleRigidWallCollisionSolver.CalculateCollisionForce(particle, rigidWall);
            calculateCollisionForce.Should().Be(expectedCollisionForce);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    new Particle(new Point2d(5, 0), 6, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(0, 10), new Point2d(0, -10)),
                    new Point2d(0,0),
                    new Vector2d(1, 0),
                },
                new object[]
                {
                    new Particle(new Point2d(-4, 4), 4, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(10, 10), new Point2d(-10, -10)),
                    new Point2d(0,0),
                    new Vector2d(0, 0),
                },
                new object[]
                {
                    new Particle(new Point2d(-1, 1), 2, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(10, 10), new Point2d(-10, -10)),
                    new Point2d(0,0),
                    new Vector2d(-0.4142136F, 0.4142136F),
                },
                new object[] //point on the line
                {
                    new Particle(new Point2d(0, 0), 1, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(10, 10), new Point2d(-10, -10)),
                    new Point2d(0,0),
                    new Vector2d(0,0),
                },
                new object[] //point on the line
                {
                    new Particle(new Point2d(5, 5), 1, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(10, 10), new Point2d(-10, -10)),
                    new Point2d(5,5),
                    new Vector2d(0,0),
                },
                new object[] //point on the line
                {
                    new Particle(new Point2d(10, 10), 1, 1, Vector2d.Zero),
                    new RigidWall(new Point2d(10, 10), new Point2d(-10, -10)),
                    new Point2d(10,10),
                    new Vector2d(0,0),
                },
                //todo db test case where mass is diffrent than 1
                //todo db test case where mass is diffrent than 1
                //todo db test case where particle hit the end of wall with offset
            };
    }
}