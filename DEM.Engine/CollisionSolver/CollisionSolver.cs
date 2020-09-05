using System;
using DEM.Engine.Elements;

namespace DEM.Engine.CollisionSolver
{
    public abstract class CollisionSolver<T1, T2> where T1 : ICollidable where T2 : ICollidable
    {
        public abstract Vector2d CalculateCollisionForce(T1 element1, T2 element2);
        public abstract bool CollisionHappened(T1 element1, T2 element2);
    }

    public static class CollisionSolver
    {
        private static readonly ParticleParticleCollisionSolver ParticleParticleCollisionSolver =
            new ParticleParticleCollisionSolver();

        private static readonly ParticleRigidWallCollisionSolver ParticleRigidWallCollisionSolver =
            new ParticleRigidWallCollisionSolver();

        public static Vector2d CalculateCollisionForce(ICollidable element1, ICollidable element2)
        {
            var collisionForce = (element1, element2) switch
            {
                _ when (element1 is Particle particle1) && (element2 is Particle particle2) =>
                    ParticleParticleCollisionSolver.CalculateCollisionForce(particle1, particle2),
                _ when (element1 is Particle particle) && (element2 is RigidWall wall) =>
                    ParticleRigidWallCollisionSolver.CalculateCollisionForce(particle, wall),
                _ when (element1 is RigidWall wall) && (element2 is Particle particle) =>
                    ParticleRigidWallCollisionSolver.CalculateCollisionForce(particle, wall),
                _ => throw new Exception("Unknown elements. Collision can not be calculated"),
            };

            return collisionForce;
        }
    }
}