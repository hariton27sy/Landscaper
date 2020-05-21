using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleGame.Graphic.Models;
using SimpleGame.GraphicEngine.Models;

namespace SimpleGame.GameCore.GameModels
{
    public class GameModel
    {
        public int Id { get; }
        public string ModelName { get; }

        private readonly IModel[] baseModels;

        public IModel this[int state] => baseModels[state];

        public static implicit operator IModel(GameModel model)
        {
            return model[0];
        }

        public int StateCount => baseModels.Length;

        public GameModel(int id, string modelName, params IModel[] states)
        {
            Id = id;
            ModelName = modelName;
            baseModels = states;
        }
    }
}