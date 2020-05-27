using OpenTK;
using SimpleGame.Graphic;

namespace SimpleGame.GameCore.Persons
{
    public interface IPlayer : ICamera
    {
        Vector3 Velocity { get; set; }
        Vector3 AbsoluteVelocity { get; }
        float Yaw { get; set; }
        float Pitch { get; set; }
        BoundaryBox BoundaryBox { get; }
    }
}