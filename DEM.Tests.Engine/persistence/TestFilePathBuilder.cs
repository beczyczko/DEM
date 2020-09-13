using DEM.Engine.Persistence;

namespace DEM.Tests.Engine.persistence
{
    internal class TestFilePathBuilder : IFilePathBuilder
    {
        public string Build(string simulationId)
        {
            return $"{simulationId}.dem";
        }
    }
}