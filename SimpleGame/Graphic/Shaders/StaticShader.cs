using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Shaders
{
    public class StaticShader : Shader
    {
        private const string VertexShaderFilename = "GraphicEngine/Shaders/colored.vert";
        private const string FragmentShader = "GraphicEngine/Shaders/simple.frag";

        private int viewMatrix, projectionMatrix, transformationMatrix;
        private int isTextured;

        public Matrix4 ViewMatrix
        {
            set => LoadMatrix4(viewMatrix, value);
        }

        public Matrix4 ProjectionMatrix
        {
            set => LoadMatrix4(projectionMatrix, value);
        }

        public Matrix4 TransformationMatrix
        {
            set => LoadMatrix4(transformationMatrix, value);
        }

        public bool IsTextured {
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
            Initialize();
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "in_color");
            BindAttribute(2, "tex_coords");
        }

        protected override void BindUniformVariables()
        {
            transformationMatrix = BindUniformVariable("transformationMatrix");
            projectionMatrix = BindUniformVariable("projectionMatrix");
            viewMatrix = BindUniformVariable("viewMatrix");
            isTextured = BindUniformVariable("isTextured");
        }
    }
}