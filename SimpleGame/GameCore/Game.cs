using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GameCore.Map;
using SimpleGame.GameCore.Worlds;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Game
    {
        private ModelsStorage storage;
        public readonly IWorld World;
        private Player player;

        public Game(IWorld world)
        {
            World = world;
        }

        public void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            
        }

        public void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            
        }

        public void OnMouse(MouseArgs args)
        {
            
        }
    }
}