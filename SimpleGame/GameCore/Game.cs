using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GameCore.Map;
using SimpleGame.GameCore.Worlds;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Game
    {
        private ModelsStorage storage;
        public readonly IWorld World;
        public readonly Player Player;

        public Game(IWorld world, Player player)
        {
            World = world;
            this.Player = player;
        }

        public void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            
        }

        public void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            
            
        }

        public void OnMouse(MouseArgs args, double x, double y)
        {

        }

        private Vector2 WorldToChunkPosition(Vector2 worldPosition)
        {
            return new Vector2((int)worldPosition.X / Chunk.Width, (int)worldPosition.Y / Chunk.Length);
        }

        private Vector2 WorldToChunkPosition(Vector3 worldPosition)
        {
            return WorldToChunkPosition(new Vector2(worldPosition.X, worldPosition.Y));
        }

        public IEntity[] OnRender()
        {
            var anchor = WorldToChunkPosition(Player.Position);
            var result = new List<IEntity>();
            foreach (var chunk in World.GetChunksInRadius(anchor, Preferences.ChunkRenderRadius))
            {
                foreach (var entity in chunk.GetEntitiesToRender())
                {
                    result.Add(entity);
                }
            }

            return result.ToArray();
        }
    }
}