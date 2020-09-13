using System.Collections.Generic;
using System.Threading.Tasks;

namespace DEM.Engine
{
    public interface IWorldSimulator
    {
        IList<World> WorldTimeSteps { get; }
        Task RunWorld(World initialStateWorld, float time, float timeStep);
    }
}