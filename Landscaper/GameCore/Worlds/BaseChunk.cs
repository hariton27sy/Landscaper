using System.Threading.Tasks;
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
        protected IModel Model;
        protected bool isModified;
        
        protected bool isPendingModel;

        public virtual IModel GetModel(ITextureStorage storage, ICamera camera)
        {
            if (Model == null)
            {
                if (!isPendingModel)
                {
                    isPendingModel = true;
                    Task.Run(() => GenerateModel(storage));
                }
            }

            return Model;
        }

        private void GenerateModel(ITextureStorage textureStorage)
        {
            Model = new ChunkModel(this, textureStorage);
            isPendingModel = false;
        }
    }
}