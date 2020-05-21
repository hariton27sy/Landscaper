using System;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Models
{
    public interface IModel : IDisposable
    {
        BeginMode DrawingMode { get; }
        
        int VerticesCount { get; }
        
        bool IsTextured { get; }
        
        IModel Start();
    }
}