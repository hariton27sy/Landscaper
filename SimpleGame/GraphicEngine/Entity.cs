using OpenTK;
using SimpleGame.GraphicEngine.Models;

namespace SimpleGame.GraphicEngine
{
    public class Entity : IEntity
    {
        public Vector3 Position
        {
            get => position;
            set
            {
                wasChanged = true;
                position = value;
            }
        }

        public float AngleX
        {
            get => angleX;
            set
            {
                wasChanged = true;
                angleX = value;
            }
        }

        public float AngleY
        {
            get => angleY;
            set
            {
                wasChanged = true;
                angleY = value;
            }
        }

        public float AngleZ
        {
            get => angleZ;
            set
            {
                wasChanged = true;
                angleZ = value;
            }
        }

        public float Scale
        {
            get => scale;
            set
            {
                wasChanged = true;
                scale = value;
            }
        }

        public Model Model { get; }

        private bool wasChanged;
        private Matrix4 translationMatrix;
        private Vector3 position;
        private float angleX, angleY, angleZ;
        private float scale;

        public Entity(Model model, Vector3 position, float rx=0, float ry=0, float rz=0, float scale=1)
        {
            Model = model;
            Position = position;
            AngleX = rx;
            AngleY = ry;
            AngleZ = rz;
            Scale = scale;
        }

        public Matrix4 TransformMatrix
        {
            get
            {
                if (!wasChanged)
                    return translationMatrix;
                wasChanged = false;
                translationMatrix = Matrix4.CreateScale(Scale) *
                                    Matrix4.CreateRotationY(AngleY) *
                                    Matrix4.CreateRotationZ(AngleZ) *
                                    Matrix4.CreateRotationX(AngleX) *
                                    Matrix4.CreateTranslation(Position);
                return translationMatrix;
            }
        }
    }
}