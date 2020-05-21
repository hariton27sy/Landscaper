using System;
using OpenTK;
using SimpleGame.Graphic;

namespace SimpleGame.GameCore.Persons
{
    public class Player : ICamera
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

        public float Yaw
        {
            get => yaw;
            set => yaw = Math.Sign(value) * Math.Max(89.9f, Math.Abs(value));
        }

        public float Pitch { get; set; }

        private float yaw;
        
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
                var cosYaw = (float) Math.Cos(MathHelper.DegreesToRadians(Yaw));
                var sinYaw = (float) Math.Sin(MathHelper.DegreesToRadians(Yaw));
                var cosPitch = (float) Math.Cos(MathHelper.DegreesToRadians(Pitch));
                var sinPitch = (float) Math.Sin(MathHelper.DegreesToRadians(Pitch));
                
                return Vector3.Normalize(new Vector3(cosYaw * cosPitch, sinYaw * cosPitch, sinPitch));
            }
        }
    }
}