using System;
using OpenTK;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Player : Camera
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

        public Player(Vector3 position = new Vector3())
        {
            CanMoveUpAndDown = false;
            Position = new Vector3(-5, 10, 0);
            Pitch = -40;
            Yaw = 0;
        }
    }
}