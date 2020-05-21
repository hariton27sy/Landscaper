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
            var bitmap = new Bitmap(textureFilename);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            return textureId;
        }
        
        
    }
}