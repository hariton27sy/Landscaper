using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IEntity
    {
        IModel GetModel(ITextureStorage storage, ICamera camera);
        Matrix4 TransformMatrix { get; }
    }
}