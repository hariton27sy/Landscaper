using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Models
{
    public class TestCube : IEntity
    {
        private static TestModel model = new TestModel();

        public IModel GetModel(TextureStorage storage, ICamera camera)
        {
            return model;
        }

        IModel IEntity.GetModel(TextureStorage storage, ICamera camera)
        {
            return GetModel(storage, camera);
        }

        public Matrix4 TransformMatrix => Matrix4.Identity;
    }

    public class TestModel : IModel
    {
        public TestModel()
        {
            vao = GlHelper.VaoCreator();
            GlHelper.VaoBinder(vao);
            indicesVbo = GlHelper.LoadIndices(indices);
            verticesVbo = GlHelper.LoadVbo(0, 3, vertices);
            GlHelper.VaoBinder(0);
        }
        
        private static readonly float[] vertices =
        {
            -0.5f, -0.5f, 0.5f,
            0.5f, -0.5f, 0.5f,
            0.5f, 0.5f, 0.5f,
            -0.5f, 0.5f, 0.5f,

            //right
            0.5f, -0.5f, 0.5f,
            0.5f, -0.5f, -0.5f,
            0.5f, 0.5f, -0.5f,
            0.5f, 0.5f, 0.5f,

            //back
            0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, 0.5f, -0.5f,
            0.5f, 0.5f, -0.5f,

            //left
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, 0.5f,
            -0.5f, 0.5f, 0.5f,
            -0.5f, 0.5f, -0.5f,

            //top
            -0.5f, 0.5f, 0.5f,
            0.5f, 0.5f, 0.5f,
            0.5f, 0.5f, -0.5f,
            -0.5f, 0.5f, -0.5f,

            //bottom
            -0.5f, -0.5f, 0.5f,
            0.5f, -0.5f, 0.5f,
            0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
        };

        private static readonly int[] indices =
        {
            0, 1, 2,
            2, 3, 0,
            4, 5, 6,
            6, 7, 4,
            8, 9, 10,
            10, 11, 8,
            12, 13, 14,
            14, 15, 12,
            16, 17, 18,
            18, 19, 16,
            20, 21, 22,
            22, 23, 20,
        };

        private int vao;
        private int indicesVbo;
        private int verticesVbo;

        public void Dispose()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(2);
        }

        public BeginMode DrawingMode => BeginMode.Triangles;
        public int VerticesCount => 36;
        public bool IsTextured => false;
        public IModel Start()
        {
            GlHelper.VaoBinder(vao);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(2);
            return this;
        }

        public void UpdateModel()
        {
            throw new System.NotImplementedException();
        }
    }

}