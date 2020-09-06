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
                    new Point2d(
                        random.Next(-100, 100),
                        random.Next(-100, 100)),
                    10,
                    1,
                    new Vector2d(NextFloat(random), NextFloat(random))))
                .ToArray();

            var rigidWalls = new RigidWall[]
            {
                new RigidWall(new Point2d(-200, -200), new Point2d(250, -200)), //top
                new RigidWall(new Point2d(250, -200), new Point2d(150, 200)), //right
                new RigidWall(new Point2d(150, 200), new Point2d(-200, 200)), //bottom
                new RigidWall(new Point2d(-200, 200), new Point2d(-200, -200)), //left
            };
            var world = new World(particles, rigidWalls);
            world.RunWorld(1000);

            return Ok(world);
        }

        [HttpGet]
        public async Task<ActionResult<World>> TwoParticlesCollision()
        {
            var particles = new[]
            {
                new Particle(new Point2d(-30, 0), 10, 1, new Vector2d(1, 0)),
                new Particle(new Point2d(30, 0), 10, 1, new Vector2d(-1, 0)),
            };
            var world = new World(particles, new RigidWall[0]);
            world.RunWorld(1000);

            return Ok(world);
        }

        private static float NextFloat(Random random)
        {
            return ((float)random.NextDouble() * 2 - 1) * 1;
        }
    }

}