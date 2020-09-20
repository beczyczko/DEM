using System.Numerics;

namespace DEM.Engine.Elements
{
    public readonly struct RigidWall : ICollidable
    {
        public RigidWall(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Vector2 P1 { get; }
        public Vector2 P2 { get; }

        public Vector2 CalculateCollisionForce(ICollidable[] interactionElements)
        {
            return CollisionSolver.CollisionSolver.CalculateCollisionForce(this, interactionElements);
        }
    }
}