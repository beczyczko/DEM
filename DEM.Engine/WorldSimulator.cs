using System.Collections.Generic;

namespace DEM.Engine
{
    public class WorldSimulator : IWorldSimulator
    {
        public IList<World> WorldTimeSteps { get; } = new List<World>();

        public void RunWorld(World initialStateWorld, float time, float timeStep)
        {
            var currentState = initialStateWorld;
            while (currentState.CurrentTime < time)
            {
                var snapshot = currentState.ProcessNextStep(timeStep);
                SaveState(snapshot);
                currentState = snapshot;
            }
        }

        private void SaveState(World snapshot)
        {
            //todo db save to file
            WorldTimeSteps.Add(snapshot);
        }
    }
}