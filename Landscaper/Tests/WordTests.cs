using NUnit.Framework;
using OpenTK;
using SimpleGame.GameCore;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.GameCore.Worlds.Dimensions;
using SimpleGame.GameCore.Worlds.Generators.Terrain;
using SimpleGame.textures;

namespace Landscaper.Tests
{
    [TestFixture]
    public class WordTests
    {
        [Test]
        public void PlainWorldTest()
        {
            var generator = new PlainTerrainGeneratorMock();
            var player = new PlayerMock();
            var world = new OverWorld(player, generator);

            for (int r = -Preferences.ChunkRenderRadius; r < Preferences.ChunkRenderRadius; r++)
            {
                for (int h = -Preferences.ChunkRenderRadius; h < Preferences.ChunkRenderRadius; h++)
                {
                    var position = new Vector2(r, h);
                    var chunk = world.GetChunk(position);

                    var x = 0;
                    var y = PlainTerrainGeneratorMock.Height;
                    var z = 0;
                    Assert.That(chunk.Map[x, y, z], 
                        Is.EqualTo(PlainTerrainGeneratorMock.Block), 
                        $"In chunk {position} in map at [{x}, {y}, {z}]");
                }
            }
        }
    }

    public class PlainTerrainGeneratorMock : ITerrainGenerator
    {
        public const int Height = 0;
        public const int Block = (int) BlockType.Grass;
        public Chunk GenerateChunk(Vector2 position)
        {
            var map = new int[Chunk.Width, Chunk.Height, Chunk.Length];
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int y = 0; y < Chunk.Length; y++)
                {
                    map[x, Height, y] = Block;
                }
            }

            return new Chunk(position, map);
        }
    }

    public class PlayerMock : IPlayer
    {
        public Matrix4 ViewMatrix { get; }
        public Vector3 Direction { get; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AbsoluteVelocity { get; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public BoundaryBox BoundaryBox { get; }
    }
}