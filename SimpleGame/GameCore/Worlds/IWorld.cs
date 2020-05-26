using System;
using System.Collections.Generic;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public interface IWorld
    {
        Chunk GetChunk(Vector2 chunkPosition);
        IEnumerable<Chunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius);
        void Update(TimeSpan delta);
    }
}