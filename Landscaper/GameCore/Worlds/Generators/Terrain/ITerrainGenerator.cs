using OpenTK;

namespace SimpleGame.GameCore.Worlds.Generators.Terrain
{
    public interface ITerrainGenerator
    {
        public Chunk GenerateChunk(Vector2 position);
    }
}