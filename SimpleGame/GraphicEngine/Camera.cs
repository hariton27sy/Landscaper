using System;
using OpenTK;
using Vector3 = OpenTK.Vector3;

namespace SimpleGame.GraphicEngine
{
    public class Camera
    {

        /// <summary>
        /// Use MoveLocalByDelta to move camera. Now this setProperty used for absolute position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Turning angle around Y-axis in degrees
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// Turning angle in degrees up and down (It can be X-axis or Z-axis)
        /// </summary>
        public float Pitch
        {
            get => pitch;
            set => pitch = Math.Sign(value) * Math.Min(89f, Math.Abs(value));
        }

        public bool CanMoveUpAndDown { get; set; } = true;

        private float pitch;

        public void MoveLocalByDelta(float deltaX, float deltaY, float deltaZ)
        {
            var delta = new Vector3();
            if (!CanMoveUpAndDown)
            {
                var radiansYaw = MathHelper.DegreesToRadians(Yaw);
                var sinYaw = Math.Sin(radiansYaw);
                var cosYaw = Math.Cos(radiansYaw);
                delta.X = (float)(deltaX * cosYaw - deltaZ * sinYaw);
                delta.Z = (float)(deltaX * sinYaw + deltaZ * cosYaw);
                delta.Y = deltaY;
            }
            else
            {
                var front = Front;
                var crossVector = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
                delta = front * deltaX + crossVector * deltaZ + Vector3.UnitY * deltaY;
            }

            Position += delta;
        }

        public void MoveLocalByDelta(Vector3 delta)
        {
            MoveLocalByDelta(delta.X, delta.Y, delta.Z);
        }

        public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Front + Position, Vector3.UnitY);

        private Vector3 Front
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
    }
}