using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Elements;
using DEM.Engine.Persistence;
using DEM.Engine.WorldSimulator;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace DEM.Tests.Engine.persistence
{
    public class FilePersistenceTests : IDisposable
    {
        private readonly FilePathBuilder _filePathBuilder = new FilePathBuilder();
        private const string SimulationId = "test";

        public FilePersistenceTests()
        {
            DeleteTestFile();
        }

        [Fact]
        public async Task CanSaveWorldSnapshot()
        {
            var world = RandomWorld(4);

            var fileWorldStateSaver = new FileWorldStateSaver(_filePathBuilder);
            await fileWorldStateSaver.SaveAsync(world, SimulationId);
        }

        [Fact]
        public async Task CanLoadWorldSnapshot()
        {
            // arrange
            var world = RandomWorld(4);
            var fileWorldStateSaver = new FileWorldStateSaver(_filePathBuilder);
            await fileWorldStateSaver.SaveAsync(world, SimulationId);
            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);

            // act
            var snapshot = fileWorldStateLoader.First(SimulationId);

            // assert
            var snapshotAsJson = JsonConvert.SerializeObject(snapshot);
            var expectedWorldAsJson = JsonConvert.SerializeObject(world);
            snapshotAsJson.Should().Be(expectedWorldAsJson);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(3, 4)]
        public async Task WorldSnapshotIsSavedAfterEveryStep(int simulationTime, int expectedSnapshotCount)
        {
            // arrange
            var world = RandomWorld(4);
            var worldSimulator = new WorldSimulator(new FileWorldStateSaver(_filePathBuilder));
            await worldSimulator.RunWorldAsync(world, new SimulationParams(simulationTime, 1, SimulationId, 1));
            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);

            // act
            var snapshots = fileWorldStateLoader.All(SimulationId).ToArray();

            // assert
            snapshots.Length.Should().Be(expectedSnapshotCount);
            var snapshotsAsJson = snapshots.Select(JsonConvert.SerializeObject);
            var expectedSnapshots = worldSimulator.WorldTimeSteps.Select(JsonConvert.SerializeObject);
            snapshotsAsJson.Should().BeEquivalentTo(expectedSnapshots);
        }

        [Fact]
        public async Task CanLoadSimulationFirstAndLastStep()
        {
            // arrange
            var world = RandomWorld(4);
            var worldSimulator = new WorldSimulator(new FileWorldStateSaver(_filePathBuilder));
            await worldSimulator.RunWorldAsync(world, new SimulationParams(3, 1, SimulationId, 1));
            var fileWorldStateLoader = new FileWorldStateLoader(_filePathBuilder);

            // act
            var firstSnapshot = fileWorldStateLoader.First(SimulationId);
            var lastSnapshot = fileWorldStateLoader.Last(SimulationId);

            // assert
            var firstSnapshotAsJson = JsonConvert.SerializeObject(firstSnapshot);
            var expectedFirstWorldAsJson = JsonConvert.SerializeObject(worldSimulator.WorldTimeSteps.First());
            firstSnapshotAsJson.Should().Be(expectedFirstWorldAsJson);

            var lastSnapshotAsJson = JsonConvert.SerializeObject(lastSnapshot);
            var expectedLastWorldAsJson = JsonConvert.SerializeObject(worldSimulator.WorldTimeSteps.Last());
            lastSnapshotAsJson.Should().Be(expectedLastWorldAsJson);
        }

        private World RandomWorld(int particleCount)
        {
            var random = new Random();
            var particles = Enumerable.Range(0, particleCount)
                .Select(i => new Particle(
                    new Vector2(
                        random.Next(-80, 80),
                        random.Next(-80, 80)),
                    5,
                    1,
                    20,
                    new Vector2(random.Next(-2, 2), random.Next(-2, 2))))
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
            var filePath = _filePathBuilder.Build(SimulationId);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}