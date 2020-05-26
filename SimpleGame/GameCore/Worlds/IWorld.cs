using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public interface IWorld
    {
        Chunk GetChunk(Vector2 chunkPosition);
        IEnumerable<Chunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius);
        void Update(TimeSpan delta);
        TextureStorage TextureStorage { get; set; }
    }
}