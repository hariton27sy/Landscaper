using OpenTK;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public abstract class BaseChunk : IEntity
    {
        public abstract bool IsModified { get; set; }
        public abstract Matrix4 TransformMatrix { get; }
        public static int Width => 16;
        public static int Height = 256;
        public static int Length => 16;
        public int[,,] Map { get; protected set; }
        public abstract IModel GetModel(ITextureStorage storage, ICamera camera);
    }
}