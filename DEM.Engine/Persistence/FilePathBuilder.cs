namespace DEM.Engine.Persistence
{
    internal class FilePathBuilder : IFilePathBuilder
    {
        public string Build(string simulationId)
        {
            return $".\\simulations\\{simulationId}.dem";
        }
    }
}