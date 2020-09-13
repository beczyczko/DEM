using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Elements;
using DEM.Engine.Persistence;
using Xunit;

namespace DEM.Tests.Engine.persistence
{
    public class FilePersistenceTests : IDisposable
    {
        private readonly TestFilePathBuilder _testFilePathBuilder = new TestFilePathBuilder();
        private const string SimulationId = "test";

        public FilePersistenceTests()
        {
            DeleteTestFile();
        }

        [Fact]
        public async Task CanSaveWorldSnapshot()
        {
            var world = RandomWorld(4);

            var fileStateSaver = new FileStateSaver(_testFilePathBuilder);
            await fileStateSaver.SaveAsync(world, SimulationId);
        }


        [Fact]
        public async Task WorldSnapshotIsSavedAfterEveryStep()
        {
            var world = RandomWorld(4);
            var worldSimulator = new WorldSimulator(new FileStateSaver(_testFilePathBuilder));
            await worldSimulator.RunWorld(world, 1, 1);
        }

        private World RandomWorld(int particleCount)
        {
            var random = new Random();
            var particles = Enumerable.Range(0, particleCount)
                .Select(i => new Particle(
                    new Point2d(
                        random.Next(-80, 80),
                        random.Next(-80, 80)),
                    5,
                    1,
                    new Vector2d(random.Next(-2, 2), random.Next(-2, 2))))
                .ToArray();
            var world = new World(particles, new RigidWall[0], 0);
            return world;
        }

        public void Dispose()
        {
            DeleteTestFile();
        }

        private void DeleteTestFile()
        {
            var filePath = _testFilePathBuilder.Build(SimulationId);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}