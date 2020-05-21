using OpenTK.Graphics.OpenGL;
using SimpleGame.GameCore.Worlds;

namespace SimpleGame.Graphic.Models
{
    public class ChunkModel : IModel
    {
        public ChunkModel(Chunk chunk)
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