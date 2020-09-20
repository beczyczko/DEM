using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Elements;
using DEM.Engine.Importers;
using DEM.Engine.Persistence;
using DEM.Engine.WorldSimulator;
using FluentAssertions;
using Xunit;

namespace DEM.Tests.Engine.BenchmarkWorlds
{
    public class BenchmarkWorlds
    {
        private readonly FilePathBuilder _filePathBuilder;
        private readonly FileWorldStateSaver _fileWorldStateSaver;

        public BenchmarkWorlds()
        {
            _filePathBuilder = new FilePathBuilder();
            _fileWorldStateSaver = new FileWorldStateSaver(_filePathBuilder);
        }

        [Fact]
        public async Task Create_particles_triangular_random_benchmark_world_init_state()
        {
            var simulationId = "particles_triangular_random_init_state";
            DeleteSimulationSnapshotsFile(simulationId);

            const string filePath = "Importers/particles_triangular_random.tsv";
            const string dataSeparator = "\t";
            var particlesCsvImporter = new ParticlesImporter();
            var particles = particlesCsvImporter.Import(filePath, dataSeparator);

            var rigidWalls = new[]
            {
                new RigidWall(new Vector2(-200, -200), new Vector2(250, -200)), //top
                new RigidWall(new Vector2(250, -200), new Vector2(200, 200)), //right
                new RigidWall(new Vector2(200, 200), new Vector2(-200, 200)), //bottom
                new RigidWall(new Vector2(-200, 200), new Vector2(-200, -200)), //left
            };
            var world = new World(particles, rigidWalls, 0);

            await _fileWorldStateSaver.SaveAsync(world, simulationId);
        }

        [Theory]
        // [InlineData(100, 0.1F, 1, 1)]
        // [InlineData(100, 0.1F, 1, 0.8)]
        [InlineData(100, 0.05F, 2, 0.7)]
        // [InlineData(1000, 0.2F, 5, 0.2)]
        // [InlineData(1000, 0.05F, 5, 0.8)]
        // [InlineData(100, 0.01F, 10, 1)]
        public async Task Run_particles_triangular_random_benchmark_world_parametrized(
            float time,
            float timeStep,
            int stepsPerSnapshot,
            float particlesBounceEfficiencyFactor)
        {
            World.ParticlesBounceFactor = particlesBounceEfficiencyFactor;

            var simulationInitStateId = "particles_triangular_random_init_state";
            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);
            var worldInitState = fileWorldStateLoader.First(simulationInitStateId);

            var simulationId = $"particles_triangular_random_T_{time}_TS_{timeStep}_SPS_{stepsPerSnapshot}_BE_{particlesBounceEfficiencyFactor}";
            DeleteSimulationSnapshotsFile(simulationId);

            var worldSimulator = new WorldSimulator(_fileWorldStateSaver);
            await worldSimulator.RunWorldAsync(worldInitState, new SimulationParams(time, timeStep, simulationId, stepsPerSnapshot));
        }

        [Fact]
        public async Task Create_particles_collision_test_init_state()
        {
            const string simulationId = "particles_collisions_test_init_state";
            DeleteSimulationSnapshotsFile(simulationId);

            var particles = new[]
            {
                new Particle(new Vector2(-15F, 0), 10, 1, 200, new Vector2(10, 0)),
                new Particle(new Vector2(15F, 0), 10, 1, 200, new Vector2(-10, 0)),
                new Particle(new Vector2(-15F, 50), 10, 1, 200, new Vector2(10, 0)),

            };
            var rigidWalls = new[]
            {
                new RigidWall(new Vector2(0, 30), new Vector2(0, 70)),
            };
            var world = new World(particles, rigidWalls, 0, 0);

            await _fileWorldStateSaver.SaveAsync(world, simulationId);
        }

        [Theory]
        [InlineData(10, 0.01F, 5, 1)]
        [InlineData(10, 0.01F, 5, 0.9F)]
        [InlineData(10, 0.005F, 10, 0.5F)]
        [InlineData(100, 0.001F, 20, 0.2F)]
        // [InlineData(1000, 0.001F, 20, 0.01F)]
        public async Task Simulate_particles_collisions_tests(
            float time,
            float timeStep,
            int stepsPerSnapshot,
            float particlesBounceEfficiencyFactor)
        {
            World.ParticlesBounceFactor = particlesBounceEfficiencyFactor;

            const string simulationInitStateId = "particles_collisions_test_init_state";
            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);
            var worldInitState = fileWorldStateLoader.First(simulationInitStateId);

            var simulationId = $"particles_collisions_test_T_{time}_TS_{timeStep}_SPS_{stepsPerSnapshot}_BE_{particlesBounceEfficiencyFactor}";
            DeleteSimulationSnapshotsFile(simulationId);

            var worldSimulator = new WorldSimulator(_fileWorldStateSaver);
            await worldSimulator.RunWorldAsync(worldInitState, new SimulationParams(time, timeStep, simulationId, stepsPerSnapshot));

            var worldFinalState = worldSimulator.WorldTimeSteps.Last();
            var particlesFinalVelocity = worldFinalState.Particles.Select(p => p.V.Length()).ToArray();
            var particlesInitVelocity = worldInitState.Particles.Select(p => p.V.Length()).ToArray();

            for (int i = 0; i < particlesInitVelocity.Length; i++)
            {
                particlesFinalVelocity[i].Should().BeInRange(
                    particlesInitVelocity[i] * particlesBounceEfficiencyFactor / 1.01F,
                    particlesInitVelocity[i] * particlesBounceEfficiencyFactor * 1.01F
                    );
            }
        }

        private void DeleteSimulationSnapshotsFile(string simulationId)
        {
            var filePath = _filePathBuilder.Build(simulationId);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}