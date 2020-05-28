using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public interface ITerrainGenerator
    {
        public Chunk GenerateChunk(Vector2 position);
    }
}