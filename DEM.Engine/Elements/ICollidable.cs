using System.Numerics;

namespace DEM.Engine.Elements
{
    public interface ICollidable
    {
        Vector2 CalculateCollisionForce(ICollidable[] interactionElements);
    }
}