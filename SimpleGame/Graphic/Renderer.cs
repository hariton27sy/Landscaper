using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimpleGame.Graphic.Models;
using SimpleGame.Graphic.Shaders;

namespace SimpleGame.Graphic
{
    public class Renderer : IRenderer
    {
        private Matrix4 projectionMatrix = Matrix4.Identity;

        public Matrix4 GetProjectionMatrix()
        {
            return projectionMatrix;
        }

        public void SetProjectionMatrix(Matrix4 matrix)
        {
            projectionMatrix = matrix;
            shader.SetProjectionMatrix(matrix);
        }

        public void SetAspect(float aspect)
        {
            SetProjectionMatrix(Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90),
                    aspect, 0.01f, 1000));
        }

        private readonly IStaticShader shader;

        public Renderer(IStaticShader shader)
        {
            this.shader = shader;
        }

        public void Start()
        {
            shader.Initialize();
            using (shader.Start())
            {
                shader.SetProjectionMatrix(GetProjectionMatrix());
            }
            GL.Enable(EnableCap.DepthTest);
        }

        public void Clear(Color color=default)
        {
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render(ICamera camera, ITextureStorage storage, IEnumerable<IEntity> entities)
        {
            using (shader.Start())
            {
                foreach (var entity in entities)
                {
                    var model = entity.GetModel(storage, camera);
                    if (model == null) 
                        continue;
                    using (model.Start())
                    {
                        shader.SetIsTextured(model.IsTextured);
                        shader.SetTransformationMatrix(entity.TransformMatrix);
                        shader.SetViewMatrix(camera.ViewMatrix);
                        GL.DrawElements(model.DrawingMode, model.VerticesCount, 
                            DrawElementsType.UnsignedInt, 0);
                    }
                }
            }
        }
    }
}