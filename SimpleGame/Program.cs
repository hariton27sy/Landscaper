using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
using OpenTK;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.GameCore.Worlds.Dimensions;
using SimpleGame.GameCore.Worlds.Generators.Environment;
using SimpleGame.Graphic.Models;
using SimpleGame.textures;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // TextureEnumGenerator.Run();
            
            var container = new StandardKernel();
            
            var seed = new Random().Next(int.MaxValue);
            
            container.Bind<IEnvironmentGenerator>().To<TreeGenerator>().WithConstructorArgument("seed", seed);
            container.Bind<IEnvironmentGenerator>().To<CactusGenerator>().WithConstructorArgument("seed", seed);
            container.Bind<ITerrainGenerator>().To<TerrainGenerator>()
                .WithConstructorArgument("seed", seed)
                .WithConstructorArgument("surfaceGenerator", new NoiseGenerator(seed, 4))
                .WithConstructorArgument("biomeGenerator", new NoiseGenerator(seed, 5));
            
            container.Bind<Vector3>().ToConstant(new Vector3(1, 100, 1));
            container.Bind<IPlayer>().To<Player>().InSingletonScope();
            container.Bind<IWorld>().To<OverWorld>();
            container.Bind<ITextureStorage>()
                .ToConstructor(_ => new TextureStorage("textures"));
            container.Bind<GameWindow>().To<Game>()
                .OnActivation(g => g.Run());
            var game = container.Get<Game>();
            game.Run();
        }
    }
}
