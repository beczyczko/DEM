using System;
using System.Linq;
using DEM.Engine;
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
                    random.Next(-80, 80),
                    random.Next(-80, 80),
                    5,
                    1,
                    new Velocity(NextFloat(random), NextFloat(random))))
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
}
