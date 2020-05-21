using SimpleGame.GameCore;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Shaders;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new Renderer(new StaticShader());
            
            var player = new Player();
            var world = new OverWorld(player);
            var window = new Game(world, player, renderer);
            window.Run();
        }
    }
}
