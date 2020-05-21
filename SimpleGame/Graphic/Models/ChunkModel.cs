using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SimpleGame.GameCore.Worlds;

namespace SimpleGame.Graphic.Models
{
    public class ChunkModel : IModel
    {
        private List<float> vertices;
        private List<float> textureCoords;
        private List<int> indices;
        
        private Chunk chunk;
        private TextureStorage storage;
        
        public ChunkModel(Chunk chunk, TextureStorage storage)
        {
            this.chunk = chunk;
            this.storage = storage;
            
            GenerateModel();
        }

        private void GenerateModel()
        {
            for (var y = 0; y < Chunk.Height; y++)
            for (int x = 0; x < Chunk.Width; x++)
            for (int z = 0; z < Chunk.Length; z++)
            {
                AddBlock(x, y, z);
            }
        }
        
        private void AddBlock(int x, int y, int z)
        {
            
        }
        
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public BeginMode DrawingMode => BeginMode.Triangles;
        public int VerticesCount { get; private set; }
        public bool IsTextured => true;
        public IModel Start()
        {
            return this;
        }
    }
}