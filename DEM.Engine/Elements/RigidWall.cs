namespace DEM.Engine.Elements
{
    //todo db should there be base class 'Element' or smth?
    public readonly struct RigidWall : ICollidable
    {
        public RigidWall(Point2d p1, Point2d p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Point2d P1 { get; }
        public Point2d P2 { get; }

        public Vector2d CalculateCollisionForce(ICollidable element)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, element);
        }
    }
}