using System.Linq;
using DEM.Engine.Elements;

namespace DEM.Engine
{
    public class World
    {
        public static float ParticlesBounceFactor = 1; //todo db make it not static

        public float CurrentTime { get; }
        public float Gravity { get; }
        public Particle[] Particles { get; }
        public RigidWall[] RigidWalls { get; }

        public World(Particle[] particles, RigidWall[] rigidWalls, float currentTime, float gravity = 0.0981F)
        {
            CurrentTime = currentTime;
            Gravity = gravity;
            Particles = particles;
            RigidWalls = rigidWalls;
        }

        public World ProcessNextStep(float timeStep)
        {
            var currentParticles = Particles;

            var particlesNewState = currentParticles.ToArray(); //copy

            var restoringForces = RestoringForceCalc(currentParticles, RigidWalls);
            //todo db Cohesion - particles <--> particle
            //todo db Cohesion - rigid line <--> particle

            ApplyForcesToParticles(ref particlesNewState, restoringForces, timeStep);

            if (Gravity != 0)
            {
                ApplyGravityForcesToParticles(ref particlesNewState, timeStep);
            }

            MoveParticles(ref particlesNewState, timeStep);

            var rigidWallsNewState = RigidWalls.ToArray(); //copy

            var worldSnapshot = new World(particlesNewState, rigidWallsNewState, CurrentTime + timeStep, Gravity);
            return worldSnapshot;
        }

        private void ApplyForcesToParticles(ref Particle[] particles, Vector2d[] forces, float timeStep)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].ApplyForce(forces[i], timeStep);
            }
        }

        private void ApplyGravityForcesToParticles(ref Particle[] particles, float timeStep)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].ApplyGravityForce(Gravity, timeStep);
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

        private void MoveParticles(ref Particle[] particles, float timeStep)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                particles[i].Move(timeStep);
            }
        }
    }
}