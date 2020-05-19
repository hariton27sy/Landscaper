using OpenTK;

namespace SimpleGame.GameCore
{
    public interface IPerson
    {
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
    }
}