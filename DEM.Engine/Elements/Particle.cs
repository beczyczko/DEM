namespace DEM.Engine.Elements
{
    public struct Particle : ICollidable
    {
        public Particle(float x, float y, float r, float m, Vector2d v)
        {
            X = x;
            Y = y;
            R = r;
            M = m;
            V = v;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public float M { get; set; }
        public Vector2d V { get; set; }

        public Vector2d RestoringForce(Particle[] interactionParticles)
        {
            var totalForce = new Vector2d();

            foreach (var interactionParticle in interactionParticles)
            {
                var interaction = CalculateCollisionForce(interactionParticle);
                totalForce = totalForce.Add(interaction);
            }

            return totalForce;
        }

        public void Move()
        {
            X += V.X * World.TimeStep;
            Y += V.Y * World.TimeStep;
        }

        public Vector2d CalculateCollisionForce(ICollidable element)
        {
            //todo db probably would be good idea to move this method from particle struct
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, element);
        }

        public void ApplyForce(Vector2d force)
        {
            var dV = new Vector2d(force.X * World.TimeStep / M, force.Y * World.TimeStep / M);
            V = V.Add(dV);
        }
    }
}