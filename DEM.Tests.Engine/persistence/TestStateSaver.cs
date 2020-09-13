using System.Threading.Tasks;
using DEM.Engine;
using DEM.Engine.Persistence;

namespace DEM.Tests.Engine.persistence
{
    internal class TestStateSaver : IStateSaver
    {
        public Task SaveAsync(World world, string simulationId)
        {
            return Task.CompletedTask;
        }
    }
}