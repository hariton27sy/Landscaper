using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using SimpleGame.GameCore.Persons;

namespace SimpleGame.GameCore.Worlds
{
    public class OverWorld : IWorld
    {
        private float gravity = 0;
        private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();
        private int seed; 
        
        public Chunk GetChunk(Vector2 chunkPosition)
        {
            if (chunks.TryGetValue(chunkPosition, out var chunk))
                return chunk;
            chunk = new Chunk(chunkPosition, GenerateNewChunk(chunkPosition));
            chunks.Add(chunkPosition, chunk);
            return chunk;
        }

        private int[,,] GenerateNewChunk(Vector2 chunkPosition)
        {
            var result = new int[Chunk.Width, Chunk.Height, Chunk.Length];
            const int grass = 5;// todo https://gamedev.ru/code/forum/?id=161884&page=64
            const int dirt = 3;
            const int stone = 4;
            const int bedrock = 0;
            const int sand = 7;
            const int water = 8;

            const int sandLevel = 64;
            const int seaLevel = 60;
            
            var n1 = new NoiseGenerator(seed);

            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int z = 0; z < Chunk.Length; z++)
                {
                    var position = new Vector2(x, z).InWorldPosition(chunkPosition);
                    var noise = n1.Noise((int) position.X, (int) position.Y);
                    var normalisedNoise = (noise + 1) / 2;
                    var height = (int)(normalisedNoise / 5 * Chunk.Height) + 50;

                    Console.WriteLine($"{x} {z} ({chunkPosition} => {position}) {height}");
                    var block = grass;
                    if (height < sandLevel)
                    {
                         block = sand;
                    }
                    result[x, height, z] = block;
                    
                    for (int i = height + 1; i < seaLevel; i++)
                    {
                        result[x, i, z] = water;    
                    }
                
                    
                    for (int i = 0; i < Math.Min(height, Chunk.Height); i++)
                    {
                        result[x, i, z] = stone;    
                    }
                }
            }
            // for (var y = 0; y < Chunk.Height; y++)
            // {
            //     for (var x = 0; x < Chunk.Width; x++)
            //     {
            //         for (var z = 0; z < Chunk.Length; z++)
            //         {
            //             var block = -1;
            //             if (y < 10)
            //                 block = dirt;
            //             if (y < 5)
            //                 block = stone;
            //             if (y == 10)
            //                 block = grass;
            //             if (y == 0)
            //                 block = bedrock;
            //             result[x, y, z] = block;
            //         }
            //     }
            // }

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

        public OverWorld(Player player, int seed)
        {
            this.player = player;
            this.seed = seed;
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