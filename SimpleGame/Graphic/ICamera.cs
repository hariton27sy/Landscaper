using OpenTK;

namespace SimpleGame.Graphic
{
    public interface ICamera
    {
        public Matrix4 ViewMatrix { get; }
        public Vector3 Direction { get; }
        public Vector3 Position { get; }
    }
}