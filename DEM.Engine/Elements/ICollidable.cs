using System.Numerics;

namespace DEM.Engine.Elements
{
    public interface ICollidable : IBoundary
    {
        Vector2 CalculateCollisionForce(ICollidable[] interactionElements);
    }
}