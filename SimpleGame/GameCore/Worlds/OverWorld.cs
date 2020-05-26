using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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
                // yield return Task.Run(() => GetChunk(anchor + translation));
                var chunk = GetChunk(anchor + translation);
                Console.WriteLine(chunk);
                if (chunk != null)
                    yield return chunk;
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

        private Vector3? GetNearestBlock(Vector3 startPos, Vector3 delta)
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
                    return new Vector3((int) result.X, (int) result.Y, (int) result.Z);
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
            Vector3? nearest;
            Vector3 prevNearest = Vector3.Zero;
            while ((nearest = GetNearestBlock(person.Position, delta)) != null && prevNearest != nearest)
            {
                // Console.WriteLine($"Nearest {nearest}");
                delta = CorrectDelta(player.Position, delta, (Vector3) nearest);
                prevNearest = (Vector3)nearest;
            }

            person.Position += delta;
        }

        private Vector3 CorrectDelta(Vector3 startPos, Vector3 delta, Vector3 blockPos)
        {
            var epsilon = 0.2f;
            var x = blockPos.X;
            var y = startPos.Y +  delta.Y / delta.X * (x - startPos.X);
            var z = startPos.Z + delta.Z / delta.X * (x - startPos.X);
            if (delta.X > 0 && (int)y == (int) blockPos.Y && (int) z == (int) blockPos.Z)
                return new Vector3(x - startPos.X - epsilon, delta.Y, delta.Z);
            x = x + 1;
            y = startPos.Y +  delta.Y / delta.X * (x - startPos.X);
            z = startPos.Z + delta.Z / delta.X * (x - startPos.X);
            if (delta.X < 0 && (int) y == (int) blockPos.Y && (int) z == (int) blockPos.Z)
                return new Vector3(x - startPos.X + epsilon, delta.Y, delta.Z);
            
            y = blockPos.Y;
            x = startPos.X +  delta.X / delta.Y * (y - startPos.Y);
            z = startPos.Z +  delta.Z / delta.Y * (y - startPos.Y);
            if (delta.Y > 0 && (int)x == (int) blockPos.X && (int) z == (int) blockPos.Z)
                return new Vector3(delta.X, y - startPos.Y - epsilon, delta.Z);
            y = y + 1;
            x = startPos.X +  delta.X / delta.Y * (y - startPos.Y);
            z = startPos.Z +  delta.Z / delta.Y * (y - startPos.Y);
            if (delta.Y < 0 && (int) x == (int) blockPos.X && (int) z == (int) blockPos.Z)
                return new Vector3(delta.X, y - startPos.Y + epsilon, delta.Z);
            
            z = blockPos.Z;
            x = startPos.X +  delta.X / delta.Z * (z - startPos.Z);
            y = startPos.Y +  delta.Y / delta.Z * (z - startPos.Z);
            if (delta.Z > 0 && (int)x == (int) blockPos.X && (int) y == (int) blockPos.Y)
                return new Vector3(delta.X, delta.Y, z - startPos.Z - epsilon);
            z = z + 1;
            x = startPos.X +  delta.X / delta.Z * (z - startPos.Z);
            y = startPos.Y +  delta.Y / delta.Z * (z - startPos.Z);
            if (delta.Z < 0 && (int) x == (int) blockPos.X && (int) y == (int) blockPos.Y)
                return new Vector3(delta.X, delta.Y, z - startPos.Z + epsilon);

            return delta;
        }
    }
}