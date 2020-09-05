using System;

namespace DEM.Engine
{
    public struct Particle
    {
        public Particle(float x, float y, float r, Velocity v)
        {
            X = x;
            Y = y;
            R = r;
            V = v;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public Velocity V { get; set; }

        public Particle NextStep(Particle[] interactionParticles)
        {
            var totalVelocityChange = new Velocity();

            foreach (var interactionParticle in interactionParticles)
            {
                var interaction = Interaction(interactionParticle);
                totalVelocityChange.Add(interaction);
            }

            var newVelocity = V;
            newVelocity.Add(totalVelocityChange);

            var nextStepParticle = new Particle(X, Y, R, newVelocity);
            nextStepParticle.Move();
            return nextStepParticle;
        }

        private void Move()
        {
            X += V.X;
            Y += V.Y;
        }

        //todo db define interaction function between 2 elements that can be easly replaced
        public Velocity Interaction(Particle par2)
        {
            if (par2.Equals(this) || !CollisionHappened(par2))
            {
                return new Velocity();
            }
            else
            {
                var deltaVx = par2.V.X - V.X; // [m/s]
                var deltaVy = par2.V.Y - V.Y; // [m/s]


                //float distance1 = sqrt(deltaX1*deltaX1 + deltaY1*deltaY1);
                var deltaX = par2.X - X; // [m]
                var deltaY = par2.Y - Y; // [m]
                var distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY); // [m]

                var topOfFraction = deltaVx * deltaX + deltaVy * deltaY * World.ParticlesBounceFactor; //top of magic formula - need better documentation // m/s * m = m^2/s

                var bounceForce = topOfFraction / (distance * distance); //m^2/s * 1/m^2 = 1/s
                return new Velocity
                {
                    X = bounceForce * deltaX, //1/s * m = m/s
                    Y = bounceForce * deltaY
                };
            }
        }

        private bool CollisionHappened(Particle particleHit)
        {
            // Velocity Diff
            // var deltaVx = particleHit.V.X - V.X;
            // var deltaVy = particleHit.V.Y - V.Y;

            // roznica polozenia w chwili zderzenia
            var deltaX = particleHit.X - X;
            var deltaY = particleHit.Y - Y;

            //float distance1 = sqrt(deltaX1*deltaX1 + deltaY1*deltaY1);
            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            var radiusSum = R + particleHit.R;

            var wasHit = distance < 0.9 * radiusSum;

            return wasHit;
        }
    }
}