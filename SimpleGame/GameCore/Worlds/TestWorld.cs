using OpenTK;
using SimpleGame.GameCore.GameModels;

namespace SimpleGame.GameCore.Worlds
{
    public class TestWorld : IWorld
    {
        private ModelsStorage models;

        public TestWorld(ModelsStorage models)
        {
            this.models = models;
        }
        
        public Chunk GetChunk(Vector2 chunkPosition)
        {
            return new Chunk(models, chunkPosition, GenerateNewChunk());
        }

        private int[,,] GenerateNewChunk()
        {
            var result = new int[Chunk.Width, Chunk.Height, Chunk.Length];
            var cobble = 1;
            var dirt = 2;
            for (var y = 0; y < Chunk.Height; y++)
            {
                for (var x = 0; x < Chunk.Width; x++)
                {
                    for (var z = 0; z < Chunk.Length; z++)
                    {
                        if (y < 10)
                            result[x, y, z] = dirt;
                        if (y < 5)
                            result[x, y, z] = cobble;
                    }
                }
            }

            return result;
        }

        public void OnCLose()
        {
        }
    }
}