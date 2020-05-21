using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IEntity
    {
        IModel Model { get; }
        Matrix4 TransformMatrix { get; }
    }
}