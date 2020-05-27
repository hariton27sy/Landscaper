using System.IO;
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
        public static int Width;
        public static int Height;
        public static int Length;
        public readonly int[,,] Map;
        public abstract IModel GetModel(ITextureStorage storage, ICamera camera);
    }

    public class Chunk : BaseChunk
    {
        private readonly Vector2 location;
        public static int Width => 16;
        public static int Height => 256;
        public static int Length => 16;
        private bool pendingModel;
        

        public readonly int[,,] Map;

        public override bool IsModified
        {
            get => isModified;
            set
            {
                isModified = value;
                if (value)
                    model?.UpdateModel();
                isModified = false;
            }
        }

        private bool isModified;
        
        private IModel model;

        public Chunk(Vector2 location, int[,,] map, ITextureStorage storage)
        {
            this.location = location;
            Map = map;
            model = new ChunkModel(this, storage);
            IsModified = true;
        }

        public override IModel GetModel(ITextureStorage storage, ICamera camera)
        {
            if (model is null)
            {
                if (!pendingModel) 
                    Task.Run(() => GenerateModel(storage));
            }
            
            return model;
        }

        private void GenerateModel(ITextureStorage textureStorage)
        {
            pendingModel = true;
            model = new ChunkModel(this, textureStorage);
            pendingModel = false;
        }

        public override Matrix4 TransformMatrix => 
            Matrix4.CreateTranslation(location.X * Width, 0, location.Y * Length);
    }
}