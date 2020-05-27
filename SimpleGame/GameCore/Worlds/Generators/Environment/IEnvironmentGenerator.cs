using System;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public interface IEnvironmentGenerator
    {
        public void AddEnvironment(Chunk chunk);
    }
}