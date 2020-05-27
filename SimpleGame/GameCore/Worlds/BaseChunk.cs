using OpenTK;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public abstract class BaseChunk : IEntity
    {
        public abstract bool IsModified { get; set; }
        public abstract Matrix4 TransformMatrix { get; }
        public static int Width;
        public static int Height;
        public static int Length;
        public readonly int[,,] Map;
        public abstract IModel GetModel(ITextureStorage storage, ICamera camera);
    }
}