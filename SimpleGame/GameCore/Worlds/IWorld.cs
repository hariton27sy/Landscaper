using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Map;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore.Worlds
{
    public interface IWorld
    {
        Chunk GetChunk(Vector2 chunkPosition);
        void OnCLose();
    }
}