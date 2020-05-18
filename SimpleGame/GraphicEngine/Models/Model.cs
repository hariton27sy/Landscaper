using System;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.GraphicEngine.Models
{
    public abstract class Model : IDisposable
    {
        protected Model(int vaoId, int verticesCount, BeginMode drawingMode)
        {
            VaoId = vaoId;
            DrawingMode = drawingMode;
            VerticesCount = verticesCount;
        }

        public int VaoId { get; }
        public BeginMode DrawingMode { get; }
        public int VerticesCount { get; }
        public abstract bool IsTextured { get; }
        
        public abstract Model Start();
        public abstract void Dispose();

    }
}