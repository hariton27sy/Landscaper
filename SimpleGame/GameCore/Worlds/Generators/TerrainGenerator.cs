using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace SimpleGame.GameCore.Worlds
{
    public class TerrainGenerator
    {
        private NoiseGenerator surfaceGenerator;
        private NoiseGenerator biomeGenerator;
        private List<IEnvironmentGenerator> environmentGenerators;
        
        private Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();
        
        public delegate double GetNoise(int x, int y);
        public TerrainGenerator(int seed)
        {
            surfaceGenerator = new NoiseGenerator(seed, 4);
            biomeGenerator = new NoiseGenerator(seed, 5);
            
            environmentGenerators = new List<IEnvironmentGenerator>();
            environmentGenerators.Add(new TreeGenerator(seed));
            environmentGenerators.Add(new CactusGenerator(seed));
        }
        
        public double[,] GetNoiseMap(GetNoise getNoise, int xOffset, int zOffset, int dx, int dz)
        {
            var map = new double[dx, dz];
            for (int x = 0; x < dx; x++)
            for (int z = 0; z < dz; z++)
                map[x, z] = getNoise(xOffset + x, zOffset + z);

            return map;
        }

        enum BiomeType
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
        
        public int[,,] GenerateChunk(Vector2 chunkPosition)
        {
            const int grass = 5; // todo https://gamedev.ru/code/forum/?id=161884&page=64
            const int dirt = 3;
            const int stone = 4;
            const int bedrock = 1;
            const int sand = 7;
            const int water = 8;
            const int snow = 9;
            const int oak = 10;
            const int cactus = 11;

            const int sandLevel = 62;
            const int seaLevel = 60;
            const int layerSize = 3;
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
                        BiomeType.Forest => grass,
                        BiomeType.Beach => sand,
                        BiomeType.Hills => snow,
                        _ => grass
                    };

                    if (height < sandLevel)
                    {
                        block = sand;
                    }

                    result[x, height, z] = block;

                    for (int i = height + 1; i < seaLevel; i++)
                    {
                        result[x, i, z] = water;
                    }


                    var stoneBorder = Math.Min(height, Chunk.Height) - layerSize;
                    for (int i = 0; i < stoneBorder; i++)
                    {
                        result[x, i, z] = stone;
                    }

                    for (int i = stoneBorder; i < stoneBorder + layerSize; i++)
                    {
                        if (block == grass)
                            result[x, i, z] = dirt;
                        else
                            result[x, i, z] = block;
                    }
                }
            }
            
            return result;
        }

        public Chunk GetChunk(Vector2 chunkPosition)
        {
            if (chunks.TryGetValue(chunkPosition, out var chunk))
                return chunk;
            chunk = new Chunk(chunkPosition, GenerateChunk(chunkPosition));
            foreach (var environmentGenerator in environmentGenerators)
            {
                environmentGenerator.AddEnvironment(chunk);   
            }
            chunks.Add(chunkPosition, chunk);
            return chunk;
        }
    }
}