namespace DEM.Engine.Persistence
{
    internal interface IFilePathBuilder
    {
        string StorageDirectory { get; }
        string Build(string simulationId);
    }
}