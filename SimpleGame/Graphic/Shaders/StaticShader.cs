using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Shaders
{
    public interface IStaticShader : IDisposable
    {
        Matrix4 ViewMatrix { set; }
        Matrix4 ProjectionMatrix { set; }
        Matrix4 TransformationMatrix { set; }
        bool IsTextured { set; }
        int ProgramId { get; }
        bool IsActive { get; }
        void BindAttributes();
        void BindUniformVariables();
        Shader Start();
        void Remove();
        void Initialize();
    }

    public class StaticShader : Shader
    {
        private const string VertexShaderFilename = "Graphic/Shaders/colored.vert";
        private const string FragmentShader = "Graphic/Shaders/simple.frag";

        private int viewMatrix, projectionMatrix, transformationMatrix;
        private int isTextured;

        public override Matrix4 ViewMatrix
        {
            set => LoadMatrix4(viewMatrix, value);
        }

        public override Matrix4 ProjectionMatrix
        {
            set => LoadMatrix4(projectionMatrix, value);
        }

        public override Matrix4 TransformationMatrix
        {
            set => LoadMatrix4(transformationMatrix, value);
        }

        public override bool IsTextured {
            set
            {
                var number = value ? 1f : 0;
                LoadFloat(isTextured, number);
            }
        }


        public StaticShader()
        {
            ShadersFilenames.Add(ShaderType.VertexShader, VertexShaderFilename);
            ShadersFilenames.Add(ShaderType.FragmentShader, FragmentShader);
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "in_color");
            BindAttribute(2, "tex_coords");
        }

        public override void BindUniformVariables()
        {
            transformationMatrix = BindUniformVariable("transformationMatrix");
            projectionMatrix = BindUniformVariable("projectionMatrix");
            viewMatrix = BindUniformVariable("viewMatrix");
            isTextured = BindUniformVariable("isTextured");
        }
    }
}