using OpenTK;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Player : Camera
    {
        public Player(Vector3 position = new Vector3())
        {
            CanMoveUpAndDown = false;
            Position = new Vector3(-5, 10, 0);
            Pitch = -40;
            Yaw = 0;
        }
    }
}