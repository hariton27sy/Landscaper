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
        

        public int[,,] Map;

        public bool IsModified { get; private set; }
        private IModel model;

        public Chunk(Vector2 location, int[,,] map)
        {
            Location = location;
            Map = map;
            
        }

        public IModel GetModel(TextureStorage storage, ICamera camera)
        {
            if (model is null)
                model = new ChunkModel(this, storage);

            return model;
        }

        public Matrix4 TransformMatrix => 
            Matrix4.CreateTranslation(Location.X * Width, 0, Location.Y * Length);
    }
}