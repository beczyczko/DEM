using System.Collections.Generic;
using System.Threading.Tasks;
using DEM.Engine.Persistence;

namespace DEM.Engine
{
    public class WorldSimulator : IWorldSimulator
    {
        private readonly IStateSaver _stateSaver;

        public WorldSimulator(IStateSaver stateSaver)
        {
            _stateSaver = stateSaver;
        }

        public IList<World> WorldTimeSteps { get; } = new List<World>();

        public async Task RunWorld(World initialStateWorld, float time, float timeStep)
        {
            var currentState = initialStateWorld;
            await SaveStateAsync(currentState);

            while (currentState.CurrentTime < time)
            {
                var snapshot = currentState.ProcessNextStep(timeStep);
                await SaveStateAsync(snapshot);
                currentState = snapshot;
            }
        }

        private async Task SaveStateAsync(World snapshot)
        {
            WorldTimeSteps.Add(snapshot);
            await _stateSaver.SaveAsync(snapshot, "test"); //todo db simulation Id
        }

        private World LoadState(string simulationId)
        {
            //todo db
            return null;
        }
    }
}