using System.Collections.Generic;

namespace DEM.Engine.Persistence
{
    public interface IFileWorldStateLoader
    {
        World First(string simulationId);
        World SnapshotByNo(string simulationId, int snapshotNo);
        World Last(string simulationId);
        IEnumerable<World> All(string simulationId);
    }
}