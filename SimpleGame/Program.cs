﻿using System;
using OpenTK;
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

            var player = new Player(new Vector3(0, 32, 0));
            var seed = new Random().Next(Int32.MaxValue);
            var terrainGenerator = new TerrainGenerator(seed);
            var world = new OverWorld(player, terrainGenerator);
            var window = new Game(world, player);
            window.Run();
        }
    }
}
