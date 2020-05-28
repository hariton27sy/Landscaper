using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimpleGame.Graphic.Models
{
    public class TextureStorage : ITextureStorage
    {
        private readonly string directory;
        private readonly Dictionary<int, Atlas> atlases = new Dictionary<int, Atlas>();
        private readonly Dictionary<int, Texture> textures = new Dictionary<int, Texture>();
        private readonly Dictionary<string, Texture> texturesByName = new Dictionary<string, Texture>();
        
        public TextureStorage(string directory)
        {
            this.directory = directory;
        }

        public Texture this[int value] => textures[value];
        public Texture this[string value] => texturesByName[value];

        public void LoadTextures()
        {
            using (var f = new FileInfo(Path.Combine(directory, ".conf")).OpenText())
            {
                var filesCount = int.Parse(f.ReadLine());
                for (var i = 0; i < filesCount; i++)
                {
                    LoadAtlas(f.ReadLine());
                }

                var modelsCount = int.Parse(f.ReadLine());
                for (var i = 0; i < modelsCount; i++)
                {
                    var info = f.ReadLine();
                    var stateCount = int.Parse(info.Split()[0]);
                    var states = new string[stateCount];
                    for (var j = 0; j < stateCount; j++)
                    {
                        states[j] = f.ReadLine();
                    }
                    LoadTexture(info, states);
                }
            }
        }

        private void LoadAtlas(string atlasProperties)
        {
            var data = atlasProperties.Split();
            var id = int.Parse(data[0]);
            var width = int.Parse(data[2]);
            var height = int.Parse(data[3]);
            var elemWidth = int.Parse(data[4]);
            var elemHeight = int.Parse(data[5]);
            var glId = GlHelper.LoadTexture(Path.Combine(directory, data[1]));
            atlases.Add(id, new Atlas(glId, width, height, elemWidth, elemHeight));
        }

        private void LoadTexture(string info, string[] statesStr)
        {
            var splitted = info.Split();
            var id = int.Parse(splitted[1]);
            var name = splitted[2];
            
            
            
            var states = statesStr.Select(ParseState).ToArray();
            textures[id] = new Texture(name, states);
            texturesByName[name] = textures[id];
        }
        
        private Texture ParseState(string state)
        {
            var splitted = state.Split();
            var vertices = new List<float>();
            var atlasId = int.Parse(splitted[0]);
            for (var i = 0; i < splitted.Length - 1; i++)
            {
                var pos = int.Parse(splitted[i + 1]);
                vertices.AddRange(atlases[atlasId][pos]);
            }

            if (splitted.Length == 4)
            {
                var top = new float[8];
                vertices.CopyTo(0, top, 0, 8);
                vertices = Enumerable.Repeat(top, 4).Append(vertices.Skip(8).ToArray()).SelectMany(t => t).ToList();
            }

            if (splitted.Length == 2)
            {
                vertices = Enumerable.Repeat(vertices, 6).SelectMany(f => f).ToList();
            }

            return new Texture(atlases[atlasId].GlAtlasId, vertices.ToArray());
        }

        ~TextureStorage()
        {
            ReleaseUnmanagedResources();
        }

        private void ReleaseUnmanagedResources()
        {
            GlHelper.VaoRemover(atlases.Values.Select(a => a.GlAtlasId).ToArray());
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    }
}