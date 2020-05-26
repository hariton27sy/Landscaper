using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using SimpleGame.GameCore.Persons;

namespace SimpleGame.GameCore.Worlds
{
    public class OverWorld : IWorld
    {
        private float gravity = 0;
        private TerrainGenerator terrainGenerator;
        
        private Player player;

        public Chunk GetChunk(Vector2 chunkPosition)
        {
            return terrainGenerator.GetChunk(chunkPosition);
        }
        
        public IEnumerable<Chunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius)
        {
            for (int dx = -chunkRenderRadius; dx <= chunkRenderRadius; dx++)
            for (int dy = -chunkRenderRadius; dy <= chunkRenderRadius; dy++)
            {
                var translation = new Vector2(dx, dy);
                yield return GetChunk(anchor + translation);
            }

        }

        public void Update(TimeSpan delta)
        {
            player.Velocity -= Vector3.UnitY * gravity * (float)delta.TotalSeconds;
            // player.Position += player.AbsoluteVelocity * (float) delta.TotalSeconds;
            TryMove(player, player.AbsoluteVelocity * (float)delta.TotalSeconds);
            // if (player.Position.Y < 0)
            //     player.Position = new Vector3(player.Position.X, 0, player.Position.Z);
        }

        public OverWorld(Player player, int seed)
        {
            this.player = player;
            terrainGenerator = new TerrainGenerator(seed);
        }

        private BoundaryBox? GetNearestBlock(Vector3 startPos, Vector3 delta)
        {
            
            var normDelta = Vector3.Normalize(delta) * 0.2f;
            var result = normDelta;
            var prevLength = delta.Length;
            while ((delta - result).Length < prevLength)
            {
                prevLength = (delta - result).Length;
                if (GetBlockId(startPos + result) != 0)
                {
                    result = startPos + result;
                    return new BoundaryBox
                    {
                        Start = new Vector3((int) result.X, (int) result.Y, (int) result.Z),
                        End = new Vector3((int) result.X + 1, (int) result.Y + 1, (int) result.Z + 1)
                    };
                }
                result += delta;
            }
            return null;
        }

        private int GetBlockId(Vector3 position)
        {
            var chunkX = (int) (position.X / Chunk.Width);
            var chunkZ = (int) (position.Z / Chunk.Length);
            var x = (int) position.X % Chunk.Width;
            var y = (int) position.Y;
            var z = (int) position.Z % Chunk.Length;
            try
            {
                return GetChunk(new Vector2(chunkX, chunkZ)).Map[x, y, z];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }
        
        private void TryMove(Player person, Vector3 delta)
        {
            BoundaryBox? nearest;
            var prevNearest = new BoundaryBox();
            while ((nearest = GetNearestBlock(person.Position, delta)) != null && 
                   !((BoundaryBox) nearest).Equals(prevNearest))
            {
                // Console.WriteLine($"Nearest {nearest}");
                var correctDelta = CorrectDelta(player.BoundaryBox, delta, (BoundaryBox) nearest);
                if (Math.Abs(correctDelta.Y - delta.Y) > 1e-10)
                        person.Velocity = new Vector3(person.Velocity.X, 0, person.Velocity.Z);
                delta = correctDelta;
            }

            person.Position += delta;
        }

        private Vector3 CorrectDelta(BoundaryBox startPos, Vector3 delta, BoundaryBox blockPos)
        {
            var epsilon = 0.2f;
            

            return delta;
        }
    }
}