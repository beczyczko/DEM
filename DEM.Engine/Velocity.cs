namespace DEM.Engine
{
    public struct Velocity
    {
        public Velocity(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public void Add(Velocity interaction)
        {
            X += interaction.X;
            Y += interaction.Y;
        }
    }
}