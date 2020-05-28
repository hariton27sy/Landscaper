using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IEntity
    {
        public IModel GetModel(ITextureStorage storage, ICamera camera);
        public Matrix4 TransformMatrix { get; }
    }
}