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
        private TerrainGenerator terrainGenerator;
        
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
            return terrainGenerator.GenerateChunk(chunkPosition);
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

        public OverWorld(Player player, TerrainGenerator terrainGenerator)
        {
            this.player = player;
            this.terrainGenerator = terrainGenerator;
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