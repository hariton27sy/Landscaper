using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.GameCore.Worlds
{
    public interface IWorld
    {
        BaseChunk GetChunk(Vector2 chunkPosition);
        IEnumerable<BaseChunk> GetChunksInRadius(Vector2 anchor, int chunkRenderRadius);
        void Update(TimeSpan delta);
        ITextureStorage TextureStorage { get; set; }
    }
}