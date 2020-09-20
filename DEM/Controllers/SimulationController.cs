using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DEM.Common.Dispatchers;
using DEM.Engine;
using DEM.Engine.Elements;
using DEM.Engine.Persistence;
using DEM.Engine.WorldSimulator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DEM.Controllers
{
    [Route("api/[controller]")]
    public class SimulationController : BaseController
    {
        private readonly IWorldSimulator _worldSimulator;
        private readonly IFileWorldStateLoader _fileWorldStateLoader;

        public SimulationController(
            IDispatcher dispatcher,
            IWorldSimulator worldSimulator,
            IFileWorldStateLoader fileWorldStateLoader) : base(dispatcher)
        {
            _worldSimulator = worldSimulator;
            _fileWorldStateLoader = fileWorldStateLoader;
        }

        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<World>>> RandomWorld()
        {
            var random = new Random();
            var particles = Enumerable.Range(0, 30)
                .Select(i => new Particle(
                    new Point2d(
                        random.Next(-100, 100),
                        random.Next(-100, 100)),
                    10,
                    1,
                    10,
                    new Vector2d(NextFloat(random), NextFloat(random))))
                .ToArray();

            var rigidWalls = new[]
            {
                new RigidWall(new Point2d(-200, -200), new Point2d(250, -200)), //top
                new RigidWall(new Point2d(250, -200), new Point2d(150, 200)), //right
                new RigidWall(new Point2d(150, 200), new Point2d(-200, 200)), //bottom
                new RigidWall(new Point2d(-200, 200), new Point2d(-200, -200)), //left
            };
            var initialStateWorld = new World(particles, rigidWalls, 0);
            await _worldSimulator.RunWorldAsync(initialStateWorld, new SimulationParams(1000, 1, "test", 1));

            return Ok(_worldSimulator.WorldTimeSteps);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<World>>> TwoParticlesCollision()
        {
            var particles = new[]
            {
                new Particle(new Point2d(-30, 0), 10, 1, 10, new Vector2d(1, 0)),
                new Particle(new Point2d(30, 0), 10, 1, 10, new Vector2d(-1, 0)),
            };
            var initialStateWorld = new World(particles, new RigidWall[0], 0, 0);
            await _worldSimulator.RunWorldAsync(initialStateWorld, new SimulationParams(1000, 1, "test", 1));

            return Ok(_worldSimulator.WorldTimeSteps);
        }

        [HttpGet("{simulationId}")]
        public ActionResult<IEnumerable<World>> BySimulationId(string simulationId)
        {
            var snapshots = _fileWorldStateLoader.All(simulationId);
            return Ok(snapshots);
        }

        [HttpPost]
        public async Task<ActionResult<SimulationResult>> Run([FromForm] SimulationParamsApi simulationParams)
        {
            var wordStatesFromFile = ReadWordStatesFromFile(simulationParams.WorldStateFile);

            if (wordStatesFromFile is null || !wordStatesFromFile.Any())
            {
                return UnprocessableEntity("Init state file contains no valid world data");
            }

            var simulationId = $"simulation_T_{simulationParams.Time}_TS_{simulationParams.TimeStep}_SPS_{simulationParams.StepsPerSnapshot}_BE_{simulationParams.ParticlesBounceEfficiencyFactor}";

            World.ParticlesBounceFactor = simulationParams.ParticlesBounceEfficiencyFactor; //todo db

            await _worldSimulator.RunWorldAsync(
                wordStatesFromFile.First(),
                new SimulationParams(
                    simulationParams.Time,
                    simulationParams.TimeStep,
                    simulationId,
                    simulationParams.StepsPerSnapshot));

            return Ok(new SimulationResult(simulationId));
        }
        
        private static float NextFloat(Random random)
        {
            return ((float)random.NextDouble() * 2 - 1) * 1;
        }

        public List<World> ReadWordStatesFromFile(IFormFile file)
        {
            var result = new List<World>();

            using var reader = new StreamReader(file.OpenReadStream());
            while (reader.Peek() >= 0)
            {
                var worldAsString = reader.ReadLine();
                if (worldAsString != null)
                {
                    var world = JsonConvert.DeserializeObject<World>(worldAsString);
                    result.Add(world);
                }
            }

            return result;
        }
    }

    public class SimulationParamsApi
    {
        public IFormFile WorldStateFile { get; set; }
        public float Time { get; set; }
        public float TimeStep { get; set; }
        public int StepsPerSnapshot { get; set; }
        public float ParticlesBounceEfficiencyFactor { get; set; }
    }

    public class SimulationResult
    {
        public SimulationResult(string simulationId)
        {
            SimulationId = simulationId;
        }

        public string SimulationId { get; }
    }
}