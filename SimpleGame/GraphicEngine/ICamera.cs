using OpenTK;

namespace SimpleGame.GraphicEngine
{
    public interface ICamera
    {
        Matrix4 ViewMatrix { get; }
    }
}