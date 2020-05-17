using System.Collections.Generic;
using OpenTK;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore.Map
{
    public class Chunk
    {
        public readonly Vector2 location;
        public static int Width => 16;
        public static int Height => 256;
        public static int Length => 16;

        public List<MapEntity> Mobs;
        public MapEntity[,,] Map;
        
        public Chunk(Vector2 location)
        {
            this.location = location;
            Map = new MapEntity[Width, Height, Length];
        }

        public IEnumerable<(MapEntity, Vector3)> GetEntitiesToRender()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    for (int y = 0; y < Length; y++)
                    {
                        var entity = Map[x, z, y];
                        if (entity != null)
                            yield return (entity, new Vector3(location.X * x * Width, z, location.Y * y * Length));
                    }
                }
            }
        }
    }
}