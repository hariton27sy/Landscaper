using System;
using SimpleGame.textures;

namespace SimpleGame.GameCore.Worlds.Generators.Environment
{
    public class CactusGenerator : IEnvironmentGenerator
    {
        private readonly int seed;
        private readonly Random random;
        private const int MaxCountInChunk = 5;
        private const int MaxLength = 5;

        public CactusGenerator(int seed)
        {
            this.seed = seed;
            random = new Random(this.seed);  
        }

        public void AddEnvironment(Chunk chunk)
        {
            var count = random.Next(MaxCountInChunk);
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
                if (chunk.Map[x, top - 1, z] != (int) BlockType.Sand)
                    continue;

                var length = random.Next(MaxLength);
                for (var i = 0; i < length; i++)
                {
                    chunk.Map[x, top + i, z] = (int) BlockType.Cactus;
                }
            }
        }
    }
}