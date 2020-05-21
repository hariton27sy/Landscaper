using OpenTK;

namespace SimpleGame.Graphic
{
    public interface ICamera
    {
        Matrix4 ViewMatrix { get; }
        Vector3 Direction { get; }
        Vector3 Position { get; }
    }
}