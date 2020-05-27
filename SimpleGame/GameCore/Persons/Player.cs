using System;
using OpenTK;
using SimpleGame.Graphic;

namespace SimpleGame.GameCore.Persons
{
    public interface IPlayer : ICamera
    {
        Vector3 Velocity { get; set; }
        Vector3 AbsoluteVelocity { get; }
        Vector3 Position { get; set; }
        float Yaw { get; set; }
        float Pitch { get; set; }
        Matrix4 ViewMatrix { get; }
        Vector3 Direction { get; }
        BoundaryBox BoundaryBox { get; }
    }

    public class Player : IPlayer
    {
        public Vector3 Velocity { get; set; }
        
        public Vector3 AbsoluteVelocity
        {
            get
            {
                var result = new Vector3();
                var cosYaw = (float) Math.Cos(MathHelper.DegreesToRadians(Yaw));
                var sinYaw = (float) Math.Sin(MathHelper.DegreesToRadians(Yaw));
                result.X = Velocity.X * cosYaw - Velocity.Z * sinYaw;
                result.Z = Velocity.X * sinYaw + Velocity.Z * cosYaw;
                result.Y = Velocity.Y;
                return result;
            }
        }
        
        public Vector3 Position { get; set; }

        public float Yaw { get; set; }
        

        public float Pitch {
            get => pitch;
            set => pitch = Math.Sign(value) * Math.Min(89f, Math.Abs(value));
        }

        private float pitch;
        
        public Player(Vector3 position = new Vector3(), float yaw=0, float pitch=0)
        {
            Position = position;
            Pitch = pitch;
            Yaw = yaw;
        }

        public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + Direction, Vector3.UnitY);

        public Vector3 Direction
        {
            get
            {
                var front = new Vector3((float) Math.Cos(MathHelper.DegreesToRadians(Pitch)) *
                                            (float) Math.Cos(MathHelper.DegreesToRadians(Yaw)),
                                         (float) Math.Sin(MathHelper.DegreesToRadians(Pitch)),
                                         (float) Math.Cos(MathHelper.DegreesToRadians(Pitch)) *
                                            (float) Math.Sin(MathHelper.DegreesToRadians(Yaw)));
                return Vector3.Normalize(front);
            }
        }
        
        public BoundaryBox BoundaryBox  => new BoundaryBox
        {
            Start = Position - new Vector3(0.5f, 1.5f, 0.5f),
            End = Position + new Vector3(0.5f, 0.5f, 0.5f)
        };
    }
}