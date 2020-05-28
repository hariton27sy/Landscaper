using System;
using OpenTK;

namespace SimpleGame.Graphic.Shaders
{
    public interface IShader : IDisposable
    {
        public int GetProgramId();
        void SetViewMatrix(Matrix4 matrix);
        void SetProjectionMatrix(Matrix4 matrix);
        void SetTransformationMatrix(Matrix4 matrix);
        void SetIsTextured(bool isTextured);
        bool IsActive { get; }
        void BindAttributes();
        void BindUniformVariables();
        IShader Start();
        void Remove();
        void Initialize();
    }
}