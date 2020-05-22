using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Persons;

namespace SimpleGame.GameCore.Worlds
{
    public class OverWorld : IWorld
    {
        private float gravity = 10;
        private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();
        
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
            result[0, 0, 0] = 2;
            
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
        private Player player;

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

        public void Update(TimeSpan delta)
        {
            player.Velocity -= Vector3.UnitY * gravity * (float)delta.TotalSeconds;
            player.Position += player.AbsoluteVelocity * (float) delta.TotalSeconds;
            if (player.Position.Y < 0)
                player.Position = new Vector3(player.Position.X, 0, player.Position.Z);
        }

        public void OnCLose()
        {
        }

        public OverWorld(Player player)
        {
            this.player = player;
        }

        private IEnumerable<Vector3> GetBlockPositionsAlongVelocityVector()
        {
            var intX = (int) player.Position.X;
            var intY = (int) player.Position.Y;
            var intZ = (int) player.Position.Z;
            var dest = player.Position + player.AbsoluteVelocity;
            var intDestX = (int) player.Position.X;
            var intDestY = (int) player.Position.Y;
            var intDestZ = (int) player.Position.Z;


            yield return Vector3.Zero;
        }
    }
}