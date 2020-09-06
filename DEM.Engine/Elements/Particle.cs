namespace DEM.Engine.Elements
{
    public struct Particle : ICollidable
    {
        public static float SpringFactor = -1F; // [N/m] //todo db how to handle different particles

        public Particle(Point2d position, float r, float m, Vector2d v)
        {
            Position = position;
            R = r;
            M = m;
            V = v;
        }

        public Point2d Position { get; set; }
        public float R { get; set; }
        public float M { get; set; }
        public Vector2d V { get; set; }

        public void Move()
        {
            Position = new Point2d(Position.X + V.X * World.TimeStep, Position.Y + V.Y * World.TimeStep);
        }

        public Vector2d CalculateCollisionForce(ICollidable[] interactionElements)
        {
            //todo db the same method is in RigidWall
            //todo db probably would be good idea to move this method from particle struct
            var totalForce = new Vector2d();

            foreach (var element in interactionElements)
            {
                var interaction = CollisionSolver.CollisionSolver.CalculateCollisionForce(this, element);
                totalForce = totalForce.Add(interaction);
            }

            return totalForce;
        }

        public void ApplyForce(Vector2d force)
        {
            var dV = new Vector2d(force.X * World.TimeStep / M, force.Y * World.TimeStep / M);
            V = V.Add(dV);
        }
    }
}