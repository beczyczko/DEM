using System.Collections.Generic;
using System.Linq;

namespace DEM.Engine
{
    public class World
    {
        public static float LeftBorderY = 100;
        public static float RightBorderY = -50;
        public static float BottomBorderX = 100;

        public static float ParticlesBounceFactor = 0.95F;

        public static float TimeStep = 1; //todo db use it

        public static float Gravity = 0.0981F;
        public float Time = 0;
        public Particle[] Particles { get; set; }
        public List<TimeState> TimeStates { get; set; } = new List<TimeState>();

        public World(Particle[] particles)
        {
            Particles = particles;
            TimeStates = new List<TimeState>
            {
                new TimeState(Time, Particles)
            };
        }

        public void ProcessNextStep()
        {
            Time++;

            var currentTimeState = TimeStates.Last();
            var currentParticles = currentTimeState.Particles;

            var particlesNewState = new Particle[currentParticles.Length];
            for (int i = 0; i < currentParticles.Length; i++)
            {
                particlesNewState[i] = currentParticles[i].NextStep(currentParticles);
            }
            TimeStates.Add(new TimeState(Time, particlesNewState));
        }

        public void RunWorld(float time)
        {
            while (Time < time)
            {
                ProcessNextStep();
            }
        }
    }
}
