using System;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public class ParticleRigidWallCollisionSolver : CollisionSolver<Particle, RigidWall>
    {
        public override Vector2d CalculateCollisionForce(Particle element1, RigidWall element2)
        {
            throw new NotImplementedException();
        }

        public override bool CollisionHappened(Particle element1, RigidWall element2)
        {
            throw new NotImplementedException();
        }
    }
}