using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimpleGame.GraphicEngine.Shaders;

namespace SimpleGame.GraphicEngine
{
    public class Renderer
    {
        private Matrix4 projectionMatrix;

        public Matrix4 ProjectionMatrix
        {
            get => projectionMatrix;
            set
            {
                projectionMatrix = value;
                shader.ProjectionMatrix = value;
            }
        }

        private ICamera camera;
        private StaticShader shader;


        public Renderer(ICamera camera, StaticShader shader, float aspect)
        {
            this.camera = camera;
            this.shader = shader;
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70), aspect, 0.01f, 1000);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Clear(Color color=default)
        {
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render(params Entity[] entities)
        {
            using (shader.Start())
            {
                foreach (var entity in entities)
                {
                    using (entity.Model.Start())
                    {
                        shader.IsTextured = entity.Model.IsTextured;
                        shader.TransformationMatrix = entity.TransformMatrix;
                        shader.ViewMatrix = camera.ViewMatrix;
                        GL.DrawElements(entity.Model.DrawingMode, entity.Model.VerticesCount, DrawElementsType.UnsignedInt, 0);
                    }
                }
            }
        }
    }
}