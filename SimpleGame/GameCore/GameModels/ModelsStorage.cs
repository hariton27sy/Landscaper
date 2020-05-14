using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleGame.GraphicEngine;
using SimpleGame.GraphicEngine.Models;
using SimpleGame.GraphicEngine.Models.Templates;

namespace SimpleGame.GameCore.GameModels
{
    public class ModelsStorage
    {
        private readonly Dictionary<int, GameModel> models = new Dictionary<int, GameModel>();
        private readonly string directory;
        private Dictionary<int, Texture> textures = new Dictionary<int, Texture>();
        private Loader loader;


        public GameModel this[int id] => models[id];

        public void AddModel(GameModel model)
        {
            models.Add(model.Id, model);
        }

        public ModelsStorage(Loader graphicLoader, string directory)
        {
            this.directory = directory;
            this.loader = graphicLoader;
            LoadModels();
        }

        private void LoadModels()
        {
            using (var f = new FileInfo(Path.Combine(directory, ".conf")).OpenText())
            {
                var filesCount = int.Parse(f.ReadLine());
                for (var i = 0; i < filesCount; i++)
                {
                    LoadTexture(f.ReadLine());
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
                    LoadModel(info, states);
                }
            }
        }

        private void LoadTexture(string textureInfo)
        {
            var data = textureInfo.Split();
            var id = int.Parse(data[0]);
            var width = int.Parse(data[2]);
            var height = int.Parse(data[3]);
            var elemWidth = int.Parse(data[4]);
            var elemHeight = int.Parse(data[5]);
            var glId = loader.LoadTexture(Path.Combine(directory, data[1]));
            textures[id] = new Texture(glId, width, height, elemWidth, elemHeight);
        }

        private void LoadModel(string info, string[] states)
        {
            var splitted = info.Split();
            var id = int.Parse(splitted[1]);
            var name = splitted[2];
            var _states = states.Select(ParseState).ToArray();
            models.Add(id, new GameModel(id, name, _states.ToArray()));
        }

        private Model ParseState(string state)
        {
            var splitted = state.Split();
            var vertices = new List<float>();
            var texId = int.Parse(splitted[0]);
            for (var i = 0; i < splitted.Length - 1; i++)
            {
                var pos = int.Parse(splitted[i + 1]);
                vertices.AddRange(textures[texId][pos]);
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

            return loader.LoadTexturedModel(ModelTemplate.Cube, vertices.ToArray(), textures[texId].TextureId);
        }
    }
}