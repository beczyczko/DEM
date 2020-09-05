namespace DEM.Engine
{
    public struct TimeState
    {
        public TimeState(float time, Particle[] particles)
        {
            Time = time;
            Particles = particles;
        }

        public float Time { get; set; }
        public Particle[] Particles { get; set; }
    }
}