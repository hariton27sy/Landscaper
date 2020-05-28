using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds.Generators.Terrain;

namespace SimpleGame.GameCore.Worlds.Dimensions
{
    public class OverWorld : WorldBase
    {
        private readonly ITerrainGenerator terrainGenerator;
        private readonly Dictionary<Vector2, BaseChunk> chunks = new Dictionary<Vector2, BaseChunk>();

        public override BaseChunk GetChunk(Vector2 chunkPosition)
        {
            if (chunks.TryGetValue(chunkPosition, out var chunk))
                return chunk;
            chunk = terrainGenerator.GenerateChunk(chunkPosition);
            if (chunk != null)
                chunks.Add(chunkPosition, chunk);
            return chunk;
        }

        public OverWorld(IPlayer player, ITerrainGenerator terrainGenerator)
        {
            Player = player;
            this.terrainGenerator = terrainGenerator;
        }
    }
}