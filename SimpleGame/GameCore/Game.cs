using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SimpleGame.GameCore.Map;

namespace SimpleGame.GameCore
{
    public class Game
    {
        private readonly World world;
        public Game(World world)
        {
            this.world = world;
        }

        public IEnumerable<(MapEntity, Vector3)> GetEntitiesToRender()
        {
            foreach (var chunk in world.chunks.Select(pair => pair.Value))
            {
                foreach (var pair in chunk.GetEntitiesToRender())
                {
                    yield return pair;
                }
            }
        }
    }
}