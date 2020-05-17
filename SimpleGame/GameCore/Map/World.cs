using System.Collections.Generic;
using OpenTK;

namespace SimpleGame.GameCore.Map
{
    public abstract class World
    {
        // todo classify
        public Dictionary<Vector2, Chunk> chunks;
    }
}