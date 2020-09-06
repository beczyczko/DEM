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

        public Vector2d CalculateCollisionForce(ICollidable[] interactionElements)
        {
            //todo db the same method is in Particle
            //todo db maybe this method is not needed in RigidWall because there will be no need to call it - need analysis
            //todo db probably would be good idea to move this method from this struct
            var totalForce = new Vector2d();

            foreach (var element in interactionElements)
            {
                var interaction = CollisionSolver.CollisionSolver.CalculateCollisionForce(this, element);
                totalForce = totalForce.Add(interaction);
            }

            return totalForce;
        }
    }
}