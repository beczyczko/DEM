using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DEM.Engine.Persistence
{
    internal class FileWorldStateLoader : IFileWorldStateLoader
    {
        private readonly IFilePathBuilder _filePathBuilder;

        public FileWorldStateLoader(IFilePathBuilder filePathBuilder)
        {
            _filePathBuilder = filePathBuilder;

        }

        public World First(string simulationId)
        {
            var filePath = _filePathBuilder.Build(simulationId);

            var snapshotAsJson = ReadNthLineFromFile(filePath, 0);
            var world = JsonConvert.DeserializeObject<World>(snapshotAsJson);

            return world;
        }

        public World SnapshotByNo(string simulationId, int snapshotNo)
        {
            var filePath = _filePathBuilder.Build(simulationId);

            var snapshotAsJson = ReadNthLineFromFile(filePath, snapshotNo);
            var world = JsonConvert.DeserializeObject<World>(snapshotAsJson);

            return world;
        }

        public World Last(string simulationId)
        {
            var filePath = _filePathBuilder.Build(simulationId);

            var snapshotAsJson = File.ReadLines(filePath).Last();
            var world = JsonConvert.DeserializeObject<World>(snapshotAsJson);

            return world;
        }

        public IEnumerable<World> All(string simulationId)
        {
            var filePath = _filePathBuilder.Build(simulationId);
            var snapshots = File.ReadLines(filePath).Select(JsonConvert.DeserializeObject<World>);
            return snapshots;
        }

        private string ReadNthLineFromFile(string filePath, int snapshotNo)
        {
            return File.ReadLines(filePath).Skip(snapshotNo).Take(1).First();
        }
    }
}