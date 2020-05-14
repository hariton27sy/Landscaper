using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleGame.GraphicEngine.Models;

namespace SimpleGame.GameCore.GameModels
{
    public class GameModel
    {
        public int Id { get; }
        public string ModelName { get; }

        private readonly Model[] baseModels;

        public Model this[int state] => baseModels[state];

        public static implicit operator Model(GameModel model)
        {
            return model[0];
        }

        public int StateCount => baseModels.Length;

        public GameModel(int id, string modelName, params Model[] states)
        {
            Id = id;
            ModelName = modelName;
            baseModels = states;
        }
    }
}