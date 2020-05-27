using System.IO;
using System.Threading.Tasks;
using OpenTK;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public class Chunk : IEntity
    {
        public readonly Vector2 Location;
        public static int Width => 16;
        public static int Height => 256;
        public static int Length => 16;
        private bool pendingModel;
        

        public int[,,] Map;

        public bool IsModified
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
            Location = location;
            Map = map;
            model = new ChunkModel(this, storage);
            IsModified = true;
        }

        public IModel GetModel(ITextureStorage storage, ICamera camera)
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

        public Matrix4 TransformMatrix => 
            Matrix4.CreateTranslation(Location.X * Width, 0, Location.Y * Length);
    }
}