using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
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

        public void AddEnvironment(Chunk chunk)
        {
            const int safeBorder = 2;
            var count = random.Next(maxTreeCountInChunk);
            for (int treeCount = 0; treeCount < count; treeCount++)
            {
                var x = random.Next(safeBorder, Chunk.Width - safeBorder);
                var z = random.Next(safeBorder, Chunk.Length - safeBorder);
                var top = Chunk.Height;
                for (; top > 0; top--)
                {
                    if (chunk.Map[x, top - 1, z] != 0)
                        break;
                }
                if (chunk.Map[x, top - 1, z] != Grass)
                    continue;
                var position = new Vector3(x, top, z);
                var tree = GetTree();
                var noExcess = TryPlaceEnvironment(tree, chunk, position);
            }
        }
        
        private bool TryPlaceEnvironment(EnvironmentObject obj, Chunk chunk, Vector3 anchor)
        {
            var noExcess = true;
            foreach (var treePart in obj.Parts)
            {
                var offset = treePart.Item1;
                var blockId = treePart.Item2;

                var x = (int) (offset.X + anchor.X);
                var y = (int) (offset.Y + anchor.Y);
                var z = (int) (offset.Z + anchor.Z);
                
                var chunkPositionOffset = (x / Chunk.Width, z / Chunk.Length);
                var positionOffset = new Vector3(x % Chunk.Width, y, z % Chunk.Length);
                
                if (chunkPositionOffset == (0, 0))
                {
                    chunk.Map[x, y, z] = blockId;
                }
            }

            return noExcess;
        }
    }
}