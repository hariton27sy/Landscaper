using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GameCore.Map;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore.Worlds
{
    public class Chunk
    {
        public readonly Vector2 Location;
        public static int Width => 16;
        public static int Height => 256;
        public static int Length => 16;

        public List<MapEntity> Mobs;
        public int[,,] Map;
        private ModelsStorage storage;

        public Chunk(ModelsStorage storage, Vector2 location, int[,,] map)
        {
            Location = location;
            Map = map;
            this.storage = storage;
        }

        public IEnumerable<IEntity> GetEntitiesToRender()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        var entity = Map[x, y, z];
                        if (entity != 0 && storage.Contains(entity))
                            yield return new Block(storage[entity], new Vector3(x * 0.5f, y * 0.5f, z * 0.5f));
                    }
                }
            }
        }
    }
}