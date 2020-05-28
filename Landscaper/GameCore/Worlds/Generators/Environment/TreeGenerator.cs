using System;
using OpenTK;

namespace SimpleGame.GameCore.Worlds.Generators.Environment
{
    public class TreeGenerator : IEnvironmentGenerator
    {
        private readonly Random random;
        private int maxTreeCountInChunk = 5;

        private const int Oak = 10;
        private const int Grass = 5;
        private const int OakLeaves = 12;
        public TreeGenerator(int seed)
        {
            random = new Random(seed);
        }

        private EnvironmentObject GetTree()
        {
            const int treeHeigth = 4;
            var tree = new EnvironmentObject();
            for (int i = 0; i < treeHeigth; i++)
            {
                var anchor = new Vector3(0, i, 0);
                tree.Parts.Add((anchor, Oak));
            }

            var crownSize = 2;
            for (int i = -crownSize; i < crownSize; i++)
            for (int j = -crownSize; j < crownSize; j++)
            for (int k = -crownSize; k < crownSize; k++)
            {
                if (i * i + j * j + k * k <= crownSize)
                {
                    var anchor = new Vector3(i, treeHeigth + j, k);
                    tree.Parts.Add((anchor, OakLeaves));
                }
            }

            return tree;
        }

        public void AddEnvironment(BaseChunk chunk)
        {
            const int safeBorder = 2;
            var count = random.Next(maxTreeCountInChunk);
            for (var treeCount = 0; treeCount < count; treeCount++)
            {
                var x = random.Next(safeBorder, BaseChunk.Width - safeBorder);
                var z = random.Next(safeBorder, BaseChunk.Length - safeBorder);
                var top = BaseChunk.Height;
                for (; top > 0; top--)
                {
                    if (chunk.Map[x, top - 1, z] != 0)
                        break;
                }
                if (chunk.Map[x, top - 1, z] != Grass)
                    continue;
                var position = new Vector3(x, top, z);
                var tree = GetTree();
                TryPlaceEnvironment(tree, chunk, position);
            }
        }
        
        private static bool TryPlaceEnvironment(EnvironmentObject obj, BaseChunk chunk, Vector3 anchor)
        {
            const bool noExcess = true;
            foreach (var (offset, blockId) in obj.Parts)
            {
                var x = (int) (offset.X + anchor.X);
                var y = (int) (offset.Y + anchor.Y);
                var z = (int) (offset.Z + anchor.Z);
                
                var chunkPositionOffset = (x / BaseChunk.Width, z / BaseChunk.Length);

                if (chunkPositionOffset == (0, 0))
                {
                    chunk.Map[x, y, z] = blockId;
                }
            }

            return noExcess;
        }
    }
}