using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace DEM.Engine.Persistence
{
    [UsedImplicitly]
    internal class FileWorldStateSaver : IStateSaver
    {
        private readonly IFilePathBuilder _filePathBuilder;

        public FileWorldStateSaver(IFilePathBuilder filePathBuilder)
        {
            _filePathBuilder = filePathBuilder;
        }

        public async Task SaveAsync(World world, string simulationId)
        {
            var filePath = _filePathBuilder.Build(simulationId);

            EnsureDirectoryExists(_filePathBuilder);
            EnsureFileExists(filePath);

            await using var file = File.AppendText(filePath);
            var worldAsJson = JsonConvert.SerializeObject(world);
            await file.WriteLineAsync(worldAsJson);
        }

        private void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
        }

        private void EnsureDirectoryExists(IFilePathBuilder filePathBuilder)
        {
            Directory.CreateDirectory(filePathBuilder.StorageDirectory);
        }
    }
}