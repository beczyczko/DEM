namespace DEM.Engine.Persistence
{
    internal interface IFilePathBuilder
    {
        string Build(string simulationId);
    }
}