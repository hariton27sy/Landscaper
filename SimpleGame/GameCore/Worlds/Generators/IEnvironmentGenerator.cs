using System;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public delegate Chunk GetChunk(Vector2 position);
    public interface IEnvironmentGenerator
    {
        public void AddEnvironment(Chunk chunk);
    }
}