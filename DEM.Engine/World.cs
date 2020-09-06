using System.Collections.Generic;
using System.Linq;
using DEM.Engine.Elements;

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
        public List<TimeState> TimeStates { get; }

        public World(Particle[] particlesInitState, RigidWall[] rigidWalls)
        {
            TimeStates = new List<TimeState>
            {
                new TimeState(Time, particlesInitState, rigidWalls)
            };
        }

        public void ProcessNextStep()
        {
            Time++;

            var currentTimeState = TimeStates.Last();
            var currentParticles = currentTimeState.Particles;

            var particlesNewState = currentParticles.ToArray(); //copy

            var restoringForces = RestoringForceCalc(currentParticles, currentTimeState.RigidWalls);
            //todo db Restoring force - infinite rigid line <--> particle
            //todo db Global Gravity force - particles
            //todo db Cohesion - particles <--> particle
            //todo db Cohesion - rigid line <--> particle

            ApplyForcesToParticles(ref particlesNewState, restoringForces);
            MoveParticles(ref particlesNewState);

            var rigidWallsCopy = currentTimeState.RigidWalls.ToArray();
            TimeStates.Add(new TimeState(Time, particlesNewState, rigidWallsCopy));
        }

        private void ApplyForcesToParticles(ref Particle[] particles, Vector2d[] forces)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].ApplyForce(forces[i]);
            }
        }

        public Vector2d[] RestoringForceCalc(Particle[] particles, RigidWall[] rigidWalls)
        {
            var collidableElements = particles
                .Select(p => p as ICollidable)
                .Concat(rigidWalls.Select(w => w as ICollidable))
                .ToArray();

            var forces = new Vector2d[particles.Length];
            for (var i = 0; i < particles.Length; i++)
            {
                forces[i] = particles[i].CalculateCollisionForce(collidableElements);
            }

            return forces;
        }

        private void MoveParticles(ref Particle[] particles)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                particles[i].Move();
            }
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