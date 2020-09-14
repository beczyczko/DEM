using System.IO;
using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Elements;
using DEM.Engine.Importers;
using DEM.Engine.Persistence;
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
                new RigidWall(new Point2d(-200, -200), new Point2d(250, -200)), //top
                new RigidWall(new Point2d(250, -200), new Point2d(200, 200)), //right
                new RigidWall(new Point2d(200, 200), new Point2d(-200, 200)), //bottom
                new RigidWall(new Point2d(-200, 200), new Point2d(-200, -200)), //left
            };
            var world = new World(particles, rigidWalls, 0);

            await _fileWorldStateSaver.SaveAsync(world, simulationId);
        }

        [Fact]
        public async Task Run_particles_triangular_random_benchmark_world()
        {
            var simulationInitStateId = "particles_triangular_random_init_state";

            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);
            var worldInitState = fileWorldStateLoader.First(simulationInitStateId);

            var simulationId = "particles_triangular_random";
            DeleteSimulationSnapshotsFile(simulationId);

            var worldSimulator = new WorldSimulator(_fileWorldStateSaver);
            await worldSimulator.RunWorldAsync(worldInitState, 100, 0.1F, simulationId);
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