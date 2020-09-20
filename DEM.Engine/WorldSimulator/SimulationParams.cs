namespace DEM.Engine.WorldSimulator
{
    public class SimulationParams
    {
        public float Time { get; }
        public float TimeStep { get; }
        public string SimulationId { get; }
        public int StepsPerSnapshot { get; }

        public SimulationParams(float time, float timeStep, string simulationId, int stepsPerSnapshot)
        {
            Time = time;
            TimeStep = timeStep;
            SimulationId = simulationId;
            StepsPerSnapshot = stepsPerSnapshot;
        }
    }
}