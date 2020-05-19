using OpenTK;
using SimpleGame.GraphicEngine;
using SimpleGame.GraphicEngine.Models;

namespace SimpleGame.GameCore.Worlds
{
    public class Block : Entity
    {
        public Block(Model model, Vector3 position) : base(model, position, 0, 0, 0, 0.5f)
        {
        }
    }
}