using System.IO;
using System.Threading.Tasks;
using OpenTK;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public class Chunk : BaseChunk
    {
        private readonly Vector2 location;
        public static int Width => 16;
        public static int Height => 256;
        public static int Length => 16;
        private bool isPendingModel;

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

        public Chunk(Vector2 location, int[,,] map)
        {
            this.location = location;
            Map = map;
            IsModified = true;
        }

        public override IModel GetModel(ITextureStorage storage, ICamera camera)
        {
            if (model == null)
            {
                if (!isPendingModel)
                {
                    isPendingModel = true;
                    Task.Run(() => GenerateModel(storage));
                }
            }

            return model;
        }

        private void GenerateModel(ITextureStorage textureStorage)
        {
            model = new ChunkModel(this, textureStorage);
            isPendingModel = false;
        }

        public override Matrix4 TransformMatrix => 
            Matrix4.CreateTranslation(location.X * Width, 0, location.Y * Length);
    }
}