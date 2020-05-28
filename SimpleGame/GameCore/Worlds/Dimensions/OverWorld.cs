using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using SimpleGame.GameCore.Persons;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public class OverWorld : IWorld
    {
        private float gravity = 0;
        private readonly ITerrainGenerator terrainGenerator;
        private readonly Dictionary<Vector2, BaseChunk> chunks = new Dictionary<Vector2, BaseChunk>();
        
        private readonly IPlayer player;
        public ITextureStorage TextureStorage { get; set; }

        public BaseChunk GetChunk(Vector2 chunkPosition)
        {
            if (chunks.TryGetValue(chunkPosition, out var chunk))
                return chunk;
            chunk = terrainGenerator.GenerateChunk(chunkPosition, TextureStorage);
            if (chunk != null)
                chunks.Add(chunkPosition, chunk);
            return chunk;
        }
        
        public IEnumerable<BaseChunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius)
        {
            for (int dx = -chunkRenderRadius; dx <= chunkRenderRadius; dx++)
            for (int dy = -chunkRenderRadius; dy <= chunkRenderRadius; dy++)
            {
                var translation = new Vector2(dx, dy);
                var chunk = GetChunk(anchor + translation);
                if (chunk != null)
                    yield return chunk;
            }

        }

        public void Update(TimeSpan delta)
        {
            if (GetBlockId(player.Position - Vector3.UnitY) == 0)
                player.Velocity -= Vector3.UnitY * gravity * (float)delta.TotalSeconds;
            // player.Position += player.AbsoluteVelocity * (float) delta.TotalSeconds;
            if (Math.Abs(player.AbsoluteVelocity.Length) > 1e-10)
                TryMove(player, player.AbsoluteVelocity * (float)delta.TotalSeconds);
            // if (player.Position.Y < 0)
            //     player.Position = new Vector3(player.Position.X, 0, player.Position.Z);
        }

        public OverWorld(IPlayer player, ITerrainGenerator terrainGenerator)
        {
            this.player = player;
            this.terrainGenerator = terrainGenerator;
        }

        

        private BoundaryBox BlockBoundary(int x, int y, int z)
        {
            const int blockThickness = 1;
            var bb = new BoundaryBox();
            bb.Start = new Vector3(x, y, z);
            bb.End = new Vector3(x + blockThickness, y + blockThickness, z + blockThickness);
            return bb;
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

        private bool IsInBlock(Vector3 offset, int x, int y, int z)
        {
            const int blockThickness = 1;
            return offset.X <= x + blockThickness && offset.Y <= y + blockThickness && offset.Z <= z + blockThickness &&
                   offset.X >= x && offset.Y >= y && offset.Z >= z;
        }

        private int GetBlockId(Vector3 position)
        {
            var chunkX = (int) (position.X / BaseChunk.Width);
            var chunkZ = (int) (position.Z / BaseChunk.Length);
            var x = (int) position.X % BaseChunk.Width;
            var y = (int) position.Y;
            var z = (int) position.Z % BaseChunk.Length;
            try
            {
                var chunk = GetChunk(new Vector2(chunkX, chunkZ));
                return chunk?.Map[x, y, z] ?? 1;
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        private BoundaryBox GetPersonBoundaryBox(Vector3 position)
        {
            const float blockThickness = 0.5f;
            
            var bb = new BoundaryBox();
            bb.Start = position - Vector3.One * blockThickness;
            var dh = new Vector3(0, 1, 0);
            bb.End = position + dh + Vector3.One * blockThickness;;
            return bb;
        }
        
        private void TryMove(IPlayer person, Vector3 delta)
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

        private float GetDeltaAlongAxis(Func<Vector3, float> axisSelector, Vector3 delta, BoundaryBox startPos,
            BoundaryBox blockPos)
        {
            var result = 0f;
            if (delta.X > 0)
                result = axisSelector(blockPos.Start) - axisSelector(startPos.End);
            if (delta.X < 0)
                result = axisSelector(startPos.Start) - axisSelector(blockPos.End);

            return result;
        }
    }
}