using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.GraphicEngine.Models
{
    public class TexturedModel : Model
    {
        public int TextureId { get; }

        public TexturedModel(int vaoId, int verticesCount, int textureId, BeginMode drawingMode = BeginMode.Triangles)
            : base(vaoId, verticesCount, drawingMode)
        {
            TextureId = textureId;
        }

        public override void Dispose()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(2);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindVertexArray(0);
        }

        public override bool IsTextured { get; } = true;

        public override Model Start()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                GL.Apple.BindVertexArray(VaoId);
            }
            else
            {
                GL.BindVertexArray(VaoId);
            }
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(2);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            return this;
        }
    }
}