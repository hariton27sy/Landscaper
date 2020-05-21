using OpenTK;

namespace SimpleGame.Graphic
{
    public interface ICamera
    {
        Matrix4 ViewMatrix { get; }
    }
}