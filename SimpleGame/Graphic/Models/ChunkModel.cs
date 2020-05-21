using System;
using System.Collections.Generic;
using NLog.LayoutRenderers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic.Models.Templates;


namespace SimpleGame.Graphic.Models
{
    public class ChunkModel : IModel
    {
        private List<float> vertices = new List<float>();
        private List<float> textureCoords = new List<float>();
        private List<int> indices = new List<int>();
        private int lastIndex;
        
        private Chunk chunk;
        private TextureStorage storage;
        private ICamera camera;

        private bool isDisposed;
        private int verticesVbo;
        private int textureVbo;
        private int indicesVbo;

        public ChunkModel(Chunk chunk, TextureStorage storage, ICamera camera)
        {
            this.chunk = chunk;
            this.storage = storage;
            this.camera = camera;
            
            GenerateModel();
        }

        private void GenerateModel()
        {
            for (var y = 0; y < Chunk.Height; y++)
            for (var x = 0; x < Chunk.Width; x++)
            for (var z = 0; z < Chunk.Length; z++)
            {
                if (chunk.Map[x, y, z] != 0)
                    AddBlock(x, y, z);
            }
        }

        private void AddBlock(int x, int y, int z)
        {
            var air = -1;
            if (chunk.Map[x, y, z] == air)
                return;
            var looking = camera.Direction;
            var blockTexture = storage[chunk.Map[x, y, z]];
            // var offset = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
            var offset = new Vector3(x, y, z);
            var toSee = new HashSet<BlockEdge>();
            
            var playerRelativePosition = new Vector3(
                camera.Position.X - chunk.Location.X * Chunk.Width + offset.X , 
                camera.Position.Y - offset.Y, 
                camera.Position.Z - chunk.Location.Y * Chunk.Length + offset.Z);
            if (playerRelativePosition.X > 0)
            {
                toSee.Add(BlockEdge.Right);
            }
            else if (playerRelativePosition.X < 0)
            {
                toSee.Add(BlockEdge.Left);
            }
            else
            {
                
            }
            if (playerRelativePosition.Y > 0)
            {
                toSee.Add(BlockEdge.Top);
            }
            else if (playerRelativePosition.Y < 0)
            {
                toSee.Add(BlockEdge.Bottom);
            }
            else
            {
                
            }
            if (playerRelativePosition.Z > 0)
            {
                toSee.Add(BlockEdge.Front);
            }
            else if (playerRelativePosition.Z < 0)
            {
                toSee.Add(BlockEdge.Back);
            }
            else
            {
                
            }

            foreach (var edge in toSee)
            {
                AddFace(BlockModel.GetEdge(edge), offset);
                textureCoords.AddRange(blockTexture.GetEdge(edge));
                AddIndices();
            }
        }

        private void AddIndices()
        {
            indices.AddRange(new []{lastIndex, lastIndex + 1, lastIndex + 2, 
                lastIndex + 2, lastIndex + 3, lastIndex});

            lastIndex += 4;
        }

        private void AddFace(Vector3[] vertices, Vector3 offset)
        {
            foreach (var vertex in vertices)
            {
                var _vertex = vertex + offset;
                this.vertices.Add(_vertex.X);
                this.vertices.Add(_vertex.Y);
                this.vertices.Add(_vertex.Z);
            }
        }

        public void Dispose()
        {
            if (isDisposed)
                return;
            
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(2);
            GL.DeleteBuffer(verticesVbo);
            GL.DeleteBuffer(textureVbo);
            GL.DeleteBuffer(indicesVbo);
            isDisposed = true;
        }

        public BeginMode DrawingMode => BeginMode.Triangles;
        public int VerticesCount => indices.Count;
        public bool IsTextured => true;
        public IModel Start()
        {
            indicesVbo = GlHelper.LoadIndices(indices.ToArray());
            verticesVbo = GlHelper.LoadVbo(0, 3, vertices.ToArray());
            textureVbo = GlHelper.LoadVbo(2, 2, textureCoords.ToArray());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(2);
            GL.BindTexture(TextureTarget.Texture2D, storage[1].AtlasGlId);
            return this;
        }

        ~ChunkModel()
        {
            Dispose();
        }
    }
}