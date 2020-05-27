using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic.Models;
using SimpleGame.textures;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            TextureEnumGenerator.Run();
            
            var seed = new Random().Next(int.MaxValue);
            var environmentGenerators = new List<IEnvironmentGenerator>
            {
                new TreeGenerator(seed), 
                new CactusGenerator(seed)
            };
            var terrainGenerator = new TerrainGenerator(seed, environmentGenerators);
            var player = new Player(new Vector3(1, 100, 1));
            var world = new OverWorld(player, seed, terrainGenerator);
            
            var textures = new TextureStorage("textures");
            var window = new Game(world, player, textures);
            window.Run();
        }
    }
}
