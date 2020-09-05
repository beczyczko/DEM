using System;

namespace DEM.Engine
{
    public struct Particle
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
                var interaction = Interaction(interactionParticle);
                totalForce = totalForce.Add(interaction);
            }

            return totalForce;
        }

        public void Move()
        {
            X += V.X * World.TimeStep;
            Y += V.Y * World.TimeStep;
        }

        //todo db define interaction function between 2 elements that can be easly replaced
        public Vector2d Interaction(Particle par2)
        {
            if (par2.Equals(this) || !CollisionHappened(par2))
            {
                return new Vector2d();
            }
            else
            {
                const float springFactor = -1F; // [N/m] //todo db how to handle different particles

                var deltaX = par2.X - X; // [m]
                var deltaY = par2.Y - Y; // [m]
                var distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY); // [m]
                var deformation = (R + par2.R - distance) / 2; // [m]
                var bounceForce = springFactor * deformation / distance; // N/m * m/m = N/m
                return new Vector2d
                {
                    X = bounceForce * deltaX, // N/m * m = N
                    Y = bounceForce * deltaY
                };
            }
        }

        private bool CollisionHappened(Particle particleHit)
        {
            var deltaX = particleHit.X - X;
            var deltaY = particleHit.Y - Y;

            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            var radiusSum = R + particleHit.R;

            var wasHit = distance < radiusSum;
            return wasHit;
        }

        public void ApplyForce(Vector2d force)
        {
            var dV = new Vector2d(force.X * World.TimeStep / M, force.Y * World.TimeStep / M);
            V = V.Add(dV);
        }
    }
}