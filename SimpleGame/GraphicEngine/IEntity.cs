using OpenTK;
using SimpleGame.GraphicEngine.Models;

namespace SimpleGame.GraphicEngine
{
    public interface IEntity
    {
        Model Model { get; }
        Matrix4 TransformMatrix { get; }
    }
}