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

        public async Task RunWorldAsync(World initialStateWorld, float time, float timeStep, string simulationId)
        {
            var currentState = initialStateWorld;
            await SaveStateAsync(currentState, simulationId);

            while (currentState.CurrentTime < time)
            {
                var snapshot = currentState.ProcessNextStep(timeStep);
                await SaveStateAsync(snapshot, simulationId);
                currentState = snapshot;
            }
        }

        private async Task SaveStateAsync(World snapshot, string simulationId)
        {
            WorldTimeSteps.Add(snapshot);
            await _stateSaver.SaveAsync(snapshot, simulationId); //todo db simulation Id
        }
    }
}