using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimpleGame.Graphic.Shaders;

namespace SimpleGame.Graphic
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

        public float Aspect
        {
            set =>
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70), 
                    value, 0.01f, 1000);
        }

        private readonly StaticShader shader;

        public Renderer(StaticShader shader)
        {
            this.shader = shader;
            GL.Enable(EnableCap.DepthTest);
        }

        public void Clear(Color color=default)
        {
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render(ICamera camera, params IEntity[] entities)
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
                        GL.DrawElements(entity.Model.DrawingMode, entity.Model.VerticesCount, 
                            DrawElementsType.UnsignedInt, 0);
                    }
                }
            }
        }
    }
}