namespace DEM.Engine
{
    public struct Point2d
    {
        public Point2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}