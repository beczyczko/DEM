using System;
using System.Linq;
using DEM.Engine;
using DEM.Engine.Elements;
using Newtonsoft.Json;
using Xunit;

namespace DEM.Tests.Engine
{
    public class TestWorld
    {
        [Fact]
        public void RandomWorldExploration()
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
            var world = new World(particles, new RigidWall[0]);
            world.RunWorld(2);
            var worldAsJson = JsonConvert.SerializeObject(world);
        }

        private static float NextFloat(Random random)
        {
            return (float)random.NextDouble() * 2 - 1;
        }
    }
}
