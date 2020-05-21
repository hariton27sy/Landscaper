using OpenTK;
using SimpleGame.GameCore.GameModels;
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
        private ModelsStorage storage;

        public Chunk(ModelsStorage storage, Vector2 location, int[,,] map)
        {
            Location = location;
            Map = map;
            this.storage = storage;
        }

        public IModel Model { get; }
        public Matrix4 TransformMatrix { get; }
    }
}