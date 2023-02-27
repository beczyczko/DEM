using System.Collections.Generic;
using System.Threading.Tasks;
using DEM.Engine.Persistence;

namespace DEM.Engine.WorldSimulator
{
    public class WorldSimulator : IWorldSimulator
    {
        private readonly IStateSaver _stateSaver;

        public WorldSimulator(IStateSaver stateSaver)
        {
            _stateSaver = stateSaver;
        }

        public IList<World> WorldTimeSteps { get; } = new List<World>();

        public async Task RunWorldAsync(World initialStateWorld, SimulationParams simulationParams)
        {
            var stepCount = 0;

            var currentState = initialStateWorld;
            await SaveStateAsync(currentState, simulationParams.SimulationId);

            while (currentState.CurrentTime < simulationParams.Time)
            {
                stepCount++;
                var snapshot = currentState.ProcessNextStep(simulationParams.TimeStep);

                currentState = snapshot;

                if (stepCount % simulationParams.StepsPerSnapshot == 0)
                {
                    await SaveStateAsync(snapshot, simulationParams.SimulationId);
                }
            }
        }

        private async Task SaveStateAsync(World snapshot, string simulationId)
        {
            WorldTimeSteps.Add(snapshot); // todo db there is always the same instance of World object, so it will be needed to create copy to maintain this functionality
            await _stateSaver.SaveAsync(snapshot, simulationId);
        }
    }
}