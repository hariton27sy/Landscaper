using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using SimpleGame.GraphicEngine.Models;
using SimpleGame.GraphicEngine.Models.Templates;
using Environment = System.Environment;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace SimpleGame.GraphicEngine
{
    public class Loader
    {
        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();
        private List<int> textures = new List<int>();
        public ColoredModel Cube { get; }
        public TexturedModel TestTexture { get; }
        public TexturedModel TestTexture2 { get; }

        private readonly Func<int> vertexArrayGenerator = GL.GenVertexArray;
        private readonly Action<int> vertexArrayBinder = GL.BindVertexArray;
        private readonly Action<int[]> vertexArrayRemover = x => GL.DeleteVertexArrays(x.Length, x);

        public Loader()
        {
            if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                vertexArrayGenerator = GL.Apple.GenVertexArray;
                vertexArrayBinder = GL.Apple.BindVertexArray;
                vertexArrayRemover = x => GL.Apple.DeleteVertexArrays(x.Length, x);
            }

            float[] colors =
            {
                //front
                0, 0, 1,
                0, 0, 1,
                0, 0, 1,
                0, 0, 1,

                //right
                0, 1, 0,
                0, 1, 0,
                0, 1, 0,
                0, 1, 0,

                //back
                0, 1, 1,
                0, 1, 1,
                0, 1, 1,
                0, 1, 1,

                //left
                1, 0, 0,
                1, 0, 0,
                1, 0, 0,
                1, 0, 0,

                //top
                1, 0, 1,
                1, 0, 1,
                1, 0, 1,
                1, 0, 1,

                //bottom
                1, 1, 0,
                1, 1, 0,
                1, 1, 0,
                1, 1, 0,
            };

            float[] textureCoords =
            {
                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,
            };

            Cube = LoadColoredModel(ModelTemplate.Cube, colors);
            TestTexture = LoadTexturedModel(ModelTemplate.Cube, textureCoords, "GraphicEngine/textures/1.jpg");
            TestTexture2 = LoadTexturedModel(ModelTemplate.Cube, textureCoords, "GraphicEngine/textures/2.png");
        }

        public ColoredModel LoadColoredModel(float[] vertices, int[] indices, float[] colors=null)
        {
            var vao = LoadBaseModel(vertices, indices);
            if (colors != null)
                LoadDataToVbo(1, 3, colors);

            GL.BindVertexArray(0);

            return new ColoredModel(vao, indices.Length);
        }
        
        public ColoredModel LoadColoredModel(ModelTemplate template, float[] colors = null)
        {
            return LoadColoredModel(template.Vertices, template.Indices, colors);
        }

        public TexturedModel LoadTexturedModel(float[] vertices, int[] indices, float[] textureCoords, string texture)
        {
            return LoadTexturedModel(vertices, indices, textureCoords, LoadTexture(texture));
        }

        public TexturedModel LoadTexturedModel(ModelTemplate template, float[] textureCoords, string textureFilename)
        {
            return LoadTexturedModel(template.Vertices, template.Indices, textureCoords, textureFilename);
        }

        public TexturedModel LoadTexturedModel(float[] vertices, int[] indices, float[] textureCoords, int textureId)
        {
            var vao = LoadBaseModel(vertices, indices);
            LoadDataToVbo(2, 2, textureCoords);

            GL.BindVertexArray(0);


            return new TexturedModel(vao, indices.Length, textureId);

        }

        public TexturedModel LoadTexturedModel(ModelTemplate template, float[] textureCoords, int textureId)
        {
            return LoadTexturedModel(template.Vertices, template.Indices, textureCoords, textureId);
        }

        /// <summary>
        /// Load Textures to buffer and return texture ID in buffer.
        /// </summary>
        /// <param name="textureFilename">FileName of the picture texture</param>
        /// <returns>Texture ID in buffer</returns>
        public int LoadTexture(string textureFilename)
        {
            var bitmap = new Bitmap(textureFilename);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            return textureID;
        }

        public void Dispose()
        {
            vertexArrayRemover(vaos.ToArray());
            GL.DeleteBuffers(vbos.Count, vbos.ToArray());
            GL.DeleteTextures(textures.Count, textures.ToArray());
        }

        private void LoadDataToVbo(int attributeId, int vertexSize, float[] data)
        {
            var vbo = GL.GenBuffer();
            vbos.Add(vbo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeId, vertexSize, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void LoadIndices(int[] indices)
        {
            var vbo = GL.GenBuffer();
            vbos.Add(vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Create base model and return vaoID
        /// </summary>
        /// <returns>VAO ID</returns>
        private int LoadBaseModel(float[] vertices, int[] indices)
        {
            var vao = vertexArrayGenerator();
            vaos.Add(vao);
            vertexArrayBinder(vao);
            LoadIndices(indices);
            LoadDataToVbo(0, 3, vertices);

            return vao;
        }
    }
}