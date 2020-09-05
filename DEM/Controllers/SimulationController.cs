using System;
using System.Linq;
using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Elements;
using Microsoft.AspNetCore.Mvc;

namespace DEM.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SimulationController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<World>> RandomWorld()
        {
            var random = new Random();
            var particles = Enumerable.Range(0, 30)
                .Select(i => new Particle(
                    random.Next(-100, 100),
                    random.Next(-100, 100),
                    10,
                    1,
                    new Vector2d(NextFloat(random), NextFloat(random))))
                .ToArray();
            var world = new World(particles);
            world.RunWorld(1000);

            return Ok(world);
        }

        [HttpGet]
        public async Task<ActionResult<World>> TwoParticlesCollision()
        {
            var random = new Random();
            var particles = new Particle[]
            {
                new Particle(-30, 0, 10, 1, new Vector2d(1, 0)),
                new Particle(30, 0, 10, 1, new Vector2d(-1, 0)),
            };
            var world = new World(particles);
            world.RunWorld(1000);

            return Ok(world);
        }

        private static float NextFloat(Random random)
        {
            return ((float)random.NextDouble() * 2 - 1) * 1;
        }
    }

}