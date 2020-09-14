using JetBrains.Annotations;

namespace DEM.Engine.Persistence
{
    [UsedImplicitly]
    internal class FilePathBuilder : IFilePathBuilder
    {
        public FilePathBuilder(string storageDirectory = ".\\simulations")
        {
            StorageDirectory = storageDirectory;
        }

        public string StorageDirectory { get; }

        public string Build(string simulationId)
        {
            return $"{StorageDirectory}\\{simulationId}.dem";
        }
    }
}