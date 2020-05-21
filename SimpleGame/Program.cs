﻿using SimpleGame.GameCore;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Shaders;
using SimpleGame.GraphicEngine;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new Renderer(new StaticShader());
            
            var world = new OverWorld();
            var player = new Player();
            var window = new Game(world, player, renderer);
            window.Run();
        }
    }
}
