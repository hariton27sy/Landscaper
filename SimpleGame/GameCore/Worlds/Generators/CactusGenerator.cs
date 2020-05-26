using System;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public class CactusGenerator : IEnvironmentGenerator
    {
        private readonly int seed;
        private readonly Random random;
        private int maxCountInChunk = 5;
        private int maxLength = 5;
        private const int Cactus = 11;
        private const int Sand = 7;

        public CactusGenerator(int seed)
        {
            this.seed = seed;
            this.random = new Random(seed);  
        }

        public void AddEnvironment(Chunk chunk)
        {
            var count = random.Next(maxCountInChunk);
            for (int treeCount = 0; treeCount < count; treeCount++)
            {
                var x = random.Next(Chunk.Width);
                var z = random.Next(Chunk.Length);
                var top = Chunk.Height;
                for (; top > 0; top--)
                {
                    if (chunk.Map[x, top - 1, z] != 0)
                        break;
                }
                if (chunk.Map[x, top - 1, z] != Sand)
                    continue;

                var length = random.Next(maxLength);
                for (var i = 0; i < length; i++)
                {
                    chunk.Map[x, top + i, z] = Cactus;
                }
            }
        }
    }
}