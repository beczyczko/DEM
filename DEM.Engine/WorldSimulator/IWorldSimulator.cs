using System.Collections.Generic;
using System.Threading.Tasks;

namespace DEM.Engine.WorldSimulator
{
    public interface IWorldSimulator
    {
        IList<World> WorldTimeSteps { get; }
        Task RunWorldAsync(World initialStateWorld, SimulationParams simulationParams);
    }
}