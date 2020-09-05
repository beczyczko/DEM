namespace DEM.Engine.Elements
{
    //todo db should there be base class 'Element' or smth?
    public readonly struct RigidWall : ICollidable
    {
        public RigidWall(Point2d begin, Point2d end)
        {
            Begin = begin;
            End = end;
        }

        public Point2d Begin { get; }
        public Point2d End { get; }

        public Vector2d CalculateCollisionForce(ICollidable element)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, element);
        }
    }
}