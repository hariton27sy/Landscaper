using OpenTK;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Player : Camera
    {
        public Player(Vector3 position = new Vector3())
        {
            CanMoveUpAndDown = false;
            Position = position;
        }
    }
}