using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.GraphicEngine.Models
{
    public class ColoredModel : Model
    {
        public ColoredModel(int vaoId, int verticesCount, BeginMode drawingMode=BeginMode.Triangles) : base(vaoId, verticesCount, drawingMode)
        {
        }

        public override bool IsTextured { get; } = false;

        public override Model Start()
        {
            GL.BindVertexArray(VaoId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            return this;
        }

        public override void Dispose()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }
}