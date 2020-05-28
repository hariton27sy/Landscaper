using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES20;
using SimpleGame.GameCore.Worlds.Generators.Environment;
using SimpleGame.Graphic.Models;
using SimpleGame.textures;

namespace SimpleGame.GameCore.Worlds
{
    public class TerrainGenerator : ITerrainGenerator
    {
        private const int SandLevel = 62;
        private const int SeaLevel = 60;
        private const int LayerSize = 3;
        
        private readonly INoiseGenerator surfaceGenerator;
        private readonly INoiseGenerator biomeGenerator;
        private readonly IEnumerable<IEnvironmentGenerator> environmentGenerators;
        
        private readonly Dictionary<Vector2, Task> generatingChunks = new Dictionary<Vector2, Task>();
        private readonly Dictionary<Vector2, Chunk> generatedChunks = new Dictionary<Vector2, Chunk>();

        private readonly object objectLock;

        private delegate double GetNoise(int x, int y);
        
        public TerrainGenerator(int seed, IEnvironmentGenerator[] environmentGenerators, INoiseGenerator surfaceGenerator, INoiseGenerator biomeGenerator)
        {
            objectLock = new object();

            this.environmentGenerators = environmentGenerators;
            this.surfaceGenerator = surfaceGenerator;
            this.biomeGenerator = biomeGenerator;
        }

        private double[,] GetNoiseMap(GetNoise getNoise, int xOffset, int zOffset, int dx, int dz)
        {
            var map = new double[dx, dz];
            for (int x = 0; x < dx; x++)
            for (int z = 0; z < dz; z++)
                map[x, z] = getNoise(xOffset + x, zOffset + z);

            return map;
        }

        private enum BiomeType
        {
            Forest,
            Beach,
            Hills
        }

        private BiomeType GetBiomeType(double possibility)
        {
            if (possibility >= 0.6)
                return BiomeType.Hills;
            if (possibility >= 0.4)
                return BiomeType.Forest;
            return BiomeType.Beach;
        }

        private int[,,] GenerateChunkMap(Vector2 chunkPosition)
        {
            var result = new int[Chunk.Width, Chunk.Height, Chunk.Length];
            var offset = chunkPosition.InWorldShift();
            var heightNoise = GetNoiseMap(surfaceGenerator.Noise, (int) offset.X, (int) offset.Y, Chunk.Width, Chunk.Length);
            var biomeNoise = GetNoiseMap(biomeGenerator.Noise, (int) offset.X, (int) offset.Y, Chunk.Width, Chunk.Length); 
            
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int z = 0; z < Chunk.Length; z++)
                {
                    var normalisedNoise = (heightNoise[x, z] + 1) / 2;
                    var height = (int) (normalisedNoise / 5 * Chunk.Height) + 50;
                    var biomPossibility = (biomeNoise[x, z] + 1) / 2;
                    var biome = GetBiomeType(biomPossibility);
                    
                    var block = biome switch
                    {
                        BiomeType.Forest => BlockType.Grass,
                        BiomeType.Beach => BlockType.Sand,
                        BiomeType.Hills => BlockType.SnowGrass,
                        _ => BlockType.Grass
                    };

                    if (height < SandLevel)
                    {
                        block = BlockType.Sand;
                    }

                    result[x, height, z] = (int) block;

                    for (int i = height + 1; i < SeaLevel; i++)
                    {
                        result[x, i, z] = (int) BlockType.Water;
                    }


                    var stoneBorder = Math.Min(height, Chunk.Height) - LayerSize;
                    for (int i = 0; i < stoneBorder; i++)
                    {
                        result[x, i, z] = (int) BlockType.Cobblestone;
                    }

                    for (int i = stoneBorder; i < stoneBorder + LayerSize; i++)
                    {
                        if (block == BlockType.Grass)
                            result[x, i, z] = (int) BlockType.Dirt;
                        else
                            result[x, i, z] = (int) block;
                    }
                }
            }
            
            return result;
        }

        public Chunk GenerateChunk(Vector2 chunkPosition)
        {
            lock (objectLock)
            {
                if (generatedChunks.TryGetValue(chunkPosition, out var chunk))
                {
                    generatedChunks.Remove(chunkPosition);
                    return chunk;
                }
            }
            
            if (!generatingChunks.ContainsKey(chunkPosition))
                generatingChunks[chunkPosition] = Task.Run(() => GenerateNewChunk(chunkPosition));
            
            return null;
        }
        
        private void GenerateNewChunk(Vector2 chunkPosition)
        {
            var map = GenerateChunkMap(chunkPosition);
            var chunk = new Chunk(chunkPosition, map);
            foreach (var environmentGenerator in environmentGenerators)
            {
                environmentGenerator.AddEnvironment(chunk);   
            }

            chunk.IsModified = true;

            lock (objectLock)
            {
                generatedChunks.Add(chunkPosition, chunk);
                generatingChunks.Remove(chunkPosition);
            }
        }
    }
}