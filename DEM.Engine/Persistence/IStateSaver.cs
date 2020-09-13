using System.Threading.Tasks;

namespace DEM.Engine.Persistence
{
    public interface IStateSaver
    {
        public Task SaveAsync(World world, string simulationId);
    }
}