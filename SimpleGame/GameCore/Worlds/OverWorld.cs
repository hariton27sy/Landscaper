using System;
using System.Collections.Generic;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public class OverWorld : IWorld
    {
        private Dictionary<Vector2, Chunk> chunks;
        
        public Chunk GetChunk(Vector2 chunkPosition)
        {
            if (chunks.TryGetValue(chunkPosition, out var chunk))
                return chunk;
            chunk = new Chunk(chunkPosition, GenerateNewChunk());
            chunks.Add(chunkPosition, chunk);
            return chunk;
        }

        private int[,,] GenerateNewChunk()
        {
            var result = new int[Chunk.Width, Chunk.Height, Chunk.Length];
            var grass = 5;// todo https://gamedev.ru/code/forum/?id=161884&page=64
            var dirt = 3;
            var stone = 4;
            var bedrock = 0;
            
            for (var y = 0; y < Chunk.Height; y++)
            {
                for (var x = 0; x < Chunk.Width; x++)
                {
                    for (var z = 0; z < Chunk.Length; z++)
                    {
                        var block = -1;
                        if (y < 10)
                            block = dirt;
                        if (y < 5)
                            block = stone;
                        if (y == 10)
                            block = grass;
                        if (y == 0)
                            block = bedrock;
                        result[x, y, z] = block;
                    }
                }
            }

            return result;
        }
        
        private Func<Vector2, float, bool> IsInCircle = (d, r) => d.X*d.X + d.Y*d.Y <= r*r;

        public IEnumerable<Chunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius)
        {
            for (int dx = -chunkRenderRadius; dx <= chunkRenderRadius; dx++)
            {
                for (int dy = -chunkRenderRadius; dy <= chunkRenderRadius; dy++)
                {
                    var translation = new Vector2(dx, dy);
                    if (IsInCircle(anchor + translation, chunkRenderRadius))
                    {
                        yield return GetChunk(anchor + translation);
                    }
                }    
            }
        }

        public void OnCLose()
        {
        }
    }
}