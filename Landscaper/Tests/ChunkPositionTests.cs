using System.Collections.Generic;
using NUnit.Framework;
using OpenTK;
using SimpleGame.GameCore;
using SimpleGame.GameCore.Worlds;

namespace Landscaper.Tests
{
    [TestFixture]
    public class ChunkPositionTests
    {
        public static IEnumerable<TestCaseData> WorldToChunkPositionTestCases
        {
            get
            {
                for (int x = 0; x < Chunk.Width; x++)
                {
                    for (int y = 0; y < Chunk.Length; y++)
                    {
                        var worldPosition = new Vector2(x, y);
                        var expected = new Vector2(0, 0);
                        yield return new TestCaseData(worldPosition, expected)
                            .SetName($"World ({x}, {y}) expected in (0, 0) Chunk");
                    }
                }
                for (int x = -Preferences.ChunkRenderRadius; x <= Preferences.ChunkRenderRadius; x++)
                {
                    for (int y = -Preferences.ChunkRenderRadius; y <= Preferences.ChunkRenderRadius; y++)
                    {
                        var worldX = x * Chunk.Width;
                        var worldY = y * Chunk.Length;
                        var worldPosition = new Vector2(worldX, worldY);
                        var expected = new Vector2(worldX / Chunk.Width, worldY / Chunk.Length);
                        yield return new TestCaseData(worldPosition, expected)
                            .SetName($"World ({worldX}, {worldY}) expected in Chunk ({worldX / Chunk.Width}, {worldY / Chunk.Length}) of radius {Preferences.ChunkRenderRadius}");
                    }
                }
            }
        }

        [TestCaseSource("WorldToChunkPositionTestCases")]
        public void PositionConversionTest(Vector2 worldPosition, Vector2 expectedChunkPosition)
        {
            var actual = worldPosition.ToChunkPosition();
            Assert.That(actual, Is.EqualTo(expectedChunkPosition));
        }   
        
        public static IEnumerable<TestCaseData> ChunkToWorldPositionTestCases
        {
            get
            {
                for (int x = -Preferences.ChunkRenderRadius; x <= Preferences.ChunkRenderRadius; x++)
                {
                    for (int y = -Preferences.ChunkRenderRadius; y <= Preferences.ChunkRenderRadius; y++)
                    {
                        var worldPosition = new Vector2(x, y);
                        var expected = new Vector2(x * Chunk.Width, y * Chunk.Length);
                        yield return new TestCaseData(worldPosition, expected)
                            .SetName($"Chunk ({x}, {y}) expected in World ({x * Chunk.Width}, {y * Chunk.Length})");
                    }
                }
            }
        }
        
        [TestCaseSource("ChunkToWorldPositionTestCases")]
        public void ChunkToWorldPositionTest(Vector2 chunk, Vector2 expectedChunkPosition)
        {
            var actual = chunk.InWorldShift();
            Assert.That(actual, Is.EqualTo(expectedChunkPosition));
        }
    }
}