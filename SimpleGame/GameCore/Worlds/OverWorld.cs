using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;
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

        private IEnumerable<Vector3> GetVertices(BoundaryBox b)
        {
            yield return new Vector3(b.Start);
            yield return new Vector3(b.Start.X, b.Start.Y, b.End.Z);
            yield return new Vector3(b.Start.X, b.End.Y, b.Start.Z);
            yield return new Vector3(b.Start.X, b.End.Y, b.End.Z);
            yield return new Vector3(b.End.X, b.Start.Y, b.Start.Z);
            yield return new Vector3(b.End.X, b.Start.Y, b.End.Z);
            yield return new Vector3(b.End.X, b.End.Y, b.Start.Z);
            yield return new Vector3(b.End.X, b.End.Y, b.End.Z);
        }

        private BoundaryBox BlockBoundary(int x, int y, int z)
        {
            const int blockThickness = 1;
            var bb = new BoundaryBox();
            bb.Start = new Vector3(x, y, z);
            bb.End = new Vector3(x + blockThickness, y + blockThickness, z + blockThickness);
            return bb;
        }
        private BoundaryBox? GetNearestBlock(BoundaryBox boundaryBox, Vector3 delta)
        {
            const int partitions = 10;
            for (int i = 0; i < partitions; i++)
            {
                foreach (var vertex in GetVertices(boundaryBox))
                {
                    var offset = (delta / partitions * i) + vertex;
                    for (int x = (int) vertex.X; x < offset.X; x++)
                    {
                        for (int y = (int) vertex.Y; y < offset.Y; y++)
                        {
                            for (int z = (int) vertex.Z; z < offset.Z; z++)
                            {
                                Console.WriteLine((offset, x, y, z));
                                if (IsInBlock(offset, x, y, z))
                                {
                                    var chunkPosition = new Vector3(x, y, z).ToChunkPosition();
                                    var chunk = GetChunk(chunkPosition);
                                    if (chunk.Map[x, y, z] != 0)
                                    {
                                        Console.WriteLine("Nearest found");
                                        return BlockBoundary(x, y, z);
                                    }
                                }
                            }
                        }
                    }
                }
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

        private BoundaryBox GetPersonBoundaryBox(Vector3 position)
        {
            const float blockThickness = 0.5f;
            
            var bb = new BoundaryBox();
            bb.Start = position - Vector3.One * blockThickness;
            var dh = new Vector3(0, 1, 0);
            bb.End = position + dh + Vector3.One * blockThickness;;
            return bb;
        }
        
        private void TryMove(Player person, Vector3 delta)
        {
            BoundaryBox? nearest;
            var prevNearest = new BoundaryBox();
            while ((nearest = GetNearestBlock(GetPersonBoundaryBox(person.Position), delta)) != null && 
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
            var deltaX = GetDeltaAlongAxis(v => v.X, delta, startPos, blockPos);
            var deltaY = GetDeltaAlongAxis(v => v.Y, delta, startPos, blockPos);
            var deltaZ = GetDeltaAlongAxis(v => v.Z, delta, startPos, blockPos);
            
            if (Math.Abs(deltaY * delta.Y) < Math.Abs(deltaX * delta.X) &&
                Math.Abs(deltaZ * delta.Z) < Math.Abs(deltaX * delta.X))
                return new Vector3(deltaX * Math.Sign(delta.X), delta.Y, delta.Z);
            
            if (Math.Abs(deltaY * delta.Y) < Math.Abs(deltaZ * delta.Z))
                return new Vector3(delta.X, delta.Y, deltaZ * Math.Sign(delta.Z));
            
            return new Vector3(delta.X, deltaY * Math.Sign(delta.Y), delta.Z);
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