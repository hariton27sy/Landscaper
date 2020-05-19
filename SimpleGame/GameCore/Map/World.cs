using System.Collections.Generic;
using OpenTK;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore.Map
{
    public abstract class World
    {
        // todo classify
        public Dictionary<Vector2, Chunk> chunks;

        public abstract Chunk GetChunk(Vector2 chunkPosition);

        public abstract IEntity CreateItemsToRender(Vector3 playerPosition);
    }
}