using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IEntity
    {
        IModel GetModel(TextureStorage storage);
        Matrix4 TransformMatrix { get; }
    }
}