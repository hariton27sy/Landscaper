using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace SimpleGame.Graphic.Models
{
    public static class GlHelper
    {
        public static readonly Func<int> VaoCreator = GL.GenVertexArray;
        public static readonly Action<int[]> VaoRemover = arrays => GL.DeleteVertexArrays(arrays.Length, arrays);
        public static readonly Action<int> VaoBinder = GL.BindVertexArray;
        
        static GlHelper()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                VaoCreator = GL.Apple.GenVertexArray;
                VaoBinder = GL.Apple.BindVertexArray;
                VaoRemover = x => GL.Apple.DeleteVertexArrays(x.Length, x);
            }
        }
        
        
        public static int LoadTexture(string textureFilename)
        {
            var textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            var bitmap = new Bitmap(textureFilename);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            return textureId;
        }

        public static int LoadVbo(int attributeIndex, int vertexDimension, float[] vertices)
        {
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, 
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeIndex, vertexDimension, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return vbo;
        }

        public static int LoadIndices(int[] indices)
        {
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, 
                BufferUsageHint.StaticDraw);

            return vbo;
        }

        public static void DeleteVbos(params int[] vbos)
        {
            if (vbos != null)
                GL.DeleteBuffers(vbos.Length, vbos);
        }
    }
}