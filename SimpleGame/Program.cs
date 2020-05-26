using System;
using OpenTK;
using OpenTK.Graphics.ES10;
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
            // var renderer = new Renderer(new StaticShader());
            
            var player = new Player(new Vector3(1, 100, 1));
            var seed = new Random().Next(Int32.MaxValue);
            var world = new OverWorld(player, seed);
            var window = new Game(world, player);
            window.Run();
        }
    }
}
