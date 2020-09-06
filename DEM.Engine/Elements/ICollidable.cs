namespace DEM.Engine.Elements
{
    public interface ICollidable
    {
        Vector2d CalculateCollisionForce(ICollidable[] interactionElements);
    }
}