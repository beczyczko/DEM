using System.Collections.Generic;

namespace DEM.Engine
{
    public interface IWorldSimulator
    {
        IList<World> WorldTimeSteps { get; }
        void RunWorld(World initialStateWorld, float time, float timeStep);
    }
}