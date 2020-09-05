using System;
using System.Linq;
using System.Threading.Tasks;
using DEM.Engine;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DEM.Controllers
{
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<World>> Stickers()
        {
            var random = new Random();
            var particles = Enumerable.Range(0, 80)
                .Select(i => new Particle(
                    random.Next(-60, 60),
                    random.Next(-60, 60),
                    5,
                    new Velocity(NextFloat(random), NextFloat(random))))
                .ToArray();
            var world = new World(particles);
            world.RunWorld(1000);
            var worldAsJson = JsonConvert.SerializeObject(world);

            return Ok(world);
        }

        private static float NextFloat(Random random)
        {
            return ((float)random.NextDouble() * 2 - 1) * 1;
        }
    }

}