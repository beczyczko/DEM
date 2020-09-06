using System;
using System.Linq;
using DEM.Engine;
using DEM.Engine.CollisionSolver;
using DEM.Engine.Elements;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace DEM.Tests.Engine
{
    public class TestWorld
    {
        [Fact]
        public void Test1()
        {
            var random = new Random();
            var particles = Enumerable.Range(0, 4)
                .Select(i => new Particle(
                    new Point2d(
                        random.Next(-80, 80),
                        random.Next(-80, 80)),
                    5,
                    1,
                    new Vector2d(NextFloat(random), NextFloat(random))))
                .ToArray();
            var world = new World(particles);
            world.RunWorld(2);
            var worldAsJson = JsonConvert.SerializeObject(world);
        }

        private static float NextFloat(Random random)
        {
            return (float)random.NextDouble() * 2 - 1;
        }
    }

    public class ParticleRigidWallCollisionSolverTests
    {
        [Fact]
        //todo db many testCases
        //todo db test calculated collision point (p3)
        public void DistanceFromParticleToLineTests()
        {
            var particleRigidWallCollisionSolver = new ParticleRigidWallCollisionSolver();
            var rigidWall = new RigidWall(new Point2d(0, 10), new Point2d(0, -10));
            var particle = new Particle(new Point2d(5, 0), 6, 1, Vector2d.Zero);
            var closestPointOfWallToParticle = particleRigidWallCollisionSolver.ClosestPointOfWallToParticle(particle, rigidWall);
            var distanceFromParticleToLine = closestPointOfWallToParticle.Distance(particle.Position);
            distanceFromParticleToLine.Should().Be(5);
        }
    }
}
