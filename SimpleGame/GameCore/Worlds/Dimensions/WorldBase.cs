using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Persons;

namespace SimpleGame.GameCore.Worlds.Dimensions
{
    public abstract class WorldBase : IWorld
    {
        private float gravity = 0;
        protected IPlayer Player;

        public abstract BaseChunk GetChunk(Vector2 chunkPosition);

        public virtual IEnumerable<BaseChunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius)
        {
            var visited = new bool[2 * chunkRenderRadius, 2 * chunkRenderRadius];
            var shifts = new Queue<(int, int)>();
            shifts.Enqueue((0, 0));
            var offsets = new List<(int, int)> {(0, -1), (0, 1), (-1, 0), (1, 0)};

            while (shifts.Count != 0)
            {
                var position = shifts.Dequeue();
                foreach (var offset in offsets)
                {
                    try
                    {
                        if (visited[chunkRenderRadius + position.Item1 + offset.Item1, chunkRenderRadius + position.Item2 + offset.Item2])
                            continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    visited[chunkRenderRadius + position.Item1 + offset.Item1, chunkRenderRadius + position.Item2 + offset.Item2] = true;
                    shifts.Enqueue((position.Item1 + offset.Item1, position.Item2 + offset.Item2));
                }
                var translation = new Vector2(position.Item1, position.Item2);
                var chunk = GetChunk(anchor + translation);
                if (chunk != null)
                    yield return chunk;
            }

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

        private void TryMove(IPlayer person, Vector3 delta)
        {
            Vector3? nearest;
            Vector3 prevNearest = Vector3.Zero;
            while ((nearest = GetNearestBlock(person.Position, delta)) != null && prevNearest != nearest)
            {
                delta = CorrectDelta(Player.Position, delta, (Vector3) nearest);
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
        
        public void Update(TimeSpan delta)
        {
            if (GetBlockId(Player.Position - Vector3.UnitY) == 0)
                Player.Velocity -= Vector3.UnitY * gravity * (float)delta.TotalSeconds;
            if (Math.Abs(Player.AbsoluteVelocity.Length) > 1e-10)
                TryMove(Player, Player.AbsoluteVelocity * (float)delta.TotalSeconds);
        }
    }
}