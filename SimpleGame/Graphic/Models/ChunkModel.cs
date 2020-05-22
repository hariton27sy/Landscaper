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

        private bool isDisposed;
        private int verticesVbo;
        private int textureVbo;
        private int indicesVbo;

        public ChunkModel(Chunk chunk, TextureStorage storage)
        {
            this.chunk = chunk;
            this.storage = storage;
            
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
        
        private bool HasNeighbourOn(int x, int y, int z, BlockEdge edge)
        {
            var dx = 0;
            var dy = 0;
            var dz = 0;
            
            switch (edge)
            {
                case BlockEdge.Right:
                    dx = 1;
                    break;
                case BlockEdge.Left:
                    dx = -1;
                    break;
                case BlockEdge.Front:
                    dz = 1;
                    break;
                case BlockEdge.Back:
                    dz = -1;
                    break;
                case BlockEdge.Top:
                    dy = 1;
                    break;
                case BlockEdge.Bottom:
                    dy = -1;
                    break;
            }

            var neighbour = 0;
            try
            {
                neighbour = chunk.Map[x + dx, y + dy, z + dy];
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }

            return neighbour != -1;
        }

        private void AddBlock(int x, int y, int z)
        {
            var air = -1;
            if (chunk.Map[x, y, z] == air)
                return;
            var blockTexture = storage[chunk.Map[x, y, z]];
            var offset = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
            // var offset = new Vector3(x, y, z);
            var toSee = new HashSet<BlockEdge>();
            
            if (!HasNeighbourOn(x, y, z, BlockEdge.Right))
                toSee.Add(BlockEdge.Right);
            if (!HasNeighbourOn(x, y, z, BlockEdge.Left))
                toSee.Add(BlockEdge.Left);
            if (!HasNeighbourOn(x, y, z, BlockEdge.Top))
                toSee.Add(BlockEdge.Top);
            if (!HasNeighbourOn(x, y, z, BlockEdge.Bottom))
                toSee.Add(BlockEdge.Bottom);
            if (!HasNeighbourOn(x, y, z, BlockEdge.Front)) 
                toSee.Add(BlockEdge.Front);
            if (!HasNeighbourOn(x, y, z, BlockEdge.Back))
                toSee.Add(BlockEdge.Back);

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
            if (chunk.IsModified)
                GenerateModel();
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