using DEM.Engine.Elements;

namespace DEM.Engine
{
    public struct TimeState
    {
        public TimeState(float time, Particle[] particles, RigidWall[] rigidWalls)
        {
            Time = time;
            Particles = particles;
            RigidWalls = rigidWalls;
        }

        public float Time { get; set; }
        public Particle[] Particles { get; set; }
        public RigidWall[] RigidWalls { get; }
    }
}