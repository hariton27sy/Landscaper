using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public class Chunk : BaseChunk
    {
        private readonly Vector2 location;

        public sealed override bool IsModified
        {
            get => isModified;
            set
            {
                isModified = value;
                if (value)
                    Model?.UpdateModel();
                isModified = false;
            }
        }

        public Chunk(Vector2 location, int[,,] map)
        {
            this.location = location;
            Map = map;
            IsModified = true;
        }
        

        public override Matrix4 TransformMatrix => 
            Matrix4.CreateTranslation(location.X * Width, 0, location.Y * Length);
    }
}