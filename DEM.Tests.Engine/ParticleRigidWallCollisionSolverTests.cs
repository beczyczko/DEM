using System.Collections.Generic;
using System.Numerics;
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
            Vector2 expectedClosestPointOfWall,
            Vector2 expectedCollisionForce)
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
                    new Particle(new Vector2(5, 0), 6, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(0, 10), new Vector2(0, -10)),
                    new Vector2(0,0),
                    new Vector2(1, 0),
                },
                new object[]
                {
                    new Particle(new Vector2(-4, 4), 4, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(10, 10), new Vector2(-10, -10)),
                    new Vector2(0,0),
                    new Vector2(0, 0),
                },
                new object[]
                {
                    new Particle(new Vector2(-1, 1), 2, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(10, 10), new Vector2(-10, -10)),
                    new Vector2(0,0),
                    new Vector2(-0.4142136F, 0.4142136F),
                },
                new object[] //point on the line
                {
                    new Particle(new Vector2(0, 0), 1, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(10, 10), new Vector2(-10, -10)),
                    new Vector2(0,0),
                    new Vector2(0,0),
                },
                new object[] //point on the line
                {
                    new Particle(new Vector2(5, 5), 1, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(10, 10), new Vector2(-10, -10)),
                    new Vector2(5,5),
                    new Vector2(0,0),
                },
                new object[] //point on the line
                {
                    new Particle(new Vector2(10, 10), 1, 1, 1, Vector2.Zero),
                    new RigidWall(new Vector2(10, 10), new Vector2(-10, -10)),
                    new Vector2(10,10),
                    new Vector2(0,0),
                },
                //todo db test case where mass is diffrent than 1
                //todo db test case where mass is diffrent than 1
                //todo db test case where particle hit the end of wall with offset
                //todo db test case DumpingFactor - best write as PBT https://github.com/fscheck/FsCheck
                //todo db test case Mass - best to write as PBT https://github.com/fscheck/FsCheck
            };
    }
}