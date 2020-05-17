using SimpleGame.GameCore;
using SimpleGame.GameCore.Map;
using SimpleGame.GameCore.Map.Worlds;
using SimpleGame.GraphicEngine;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new OverWorld();
            var game = new Game(map);
            var window = new TestWindow(game);
            window.Run();
        }
    }
}
