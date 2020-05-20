using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GameCore.Worlds;
using SimpleGame.GraphicEngine;

namespace SimpleGame.GameCore
{
    public class Game
    {
        private ModelsStorage storage;
        public readonly IWorld World;
        public readonly Player Player;

        private DateTime previousTime;

        public bool IsMouseFixed { get; private set; }

        public Game(IWorld world, Player player)
        {
            World = world;
            Player = player;
            previousTime = DateTime.Now;
        }

        public void UpdateState()
        {
            var currTime = DateTime.Now;
            var delta = currTime - previousTime;
            previousTime = currTime;
            Player.Position += Player.AbsoluteVelocity * (float) delta.TotalSeconds;
        }

        public void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            Vector3 delta = Vector3.Zero;
            switch (args.Key)
            {
                case Key.Left:
                case Key.A:
                    Player.Velocity -= Vector3.UnitZ;
                    break;
                case Key.Right:
                case Key.D:
                    Player.Velocity += Vector3.UnitZ;
                    break;
                case Key.Up:
                case Key.W:
                    Player.Velocity += Vector3.UnitX;
                    break;
                case Key.Down:
                case Key.S:
                    Player.Velocity -= Vector3.UnitX;
                    break;
                case Key.ControlLeft:
                case Key.ControlRight:
                    Player.MoveLocalByDelta(new Vector3(0,-0.1f,0));
                    break;
                case Key.P:
                    IsMouseFixed = !IsMouseFixed;
                    break;
                default:
                    Console.WriteLine($"Unknown char '{args.Key}'");
                    break;
            }
        }

        public void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Left:
                case Key.A:
                    Player.Velocity = new Vector3(Player.Velocity.X, Player.Velocity.Y, 0);
                    break;
                case Key.Right:
                case Key.D:
                    Player.Velocity = new Vector3(Player.Velocity.X, Player.Velocity.Y, 0);
                    break;
                case Key.Up:
                case Key.W:
                    Player.Velocity = new Vector3(0, Player.Velocity.Y, Player.Velocity.Z);
                    break;
                case Key.Down:
                case Key.S:
                    Player.Velocity = new Vector3(0, Player.Velocity.Y, Player.Velocity.Z);
                    break;
                case Key.ControlLeft:
                case Key.ControlRight:
                    Player.MoveLocalByDelta(new Vector3(0, -0.1f, 0));
                    break;
            }

        }

        public void OnMouse(MouseArgs args)
        {
            if (IsMouseFixed)
            {
                Player.Pitch -= args.DeltaY * Preferences.Sensibility;
                Player.Yaw += args.DeltaX * Preferences.Sensibility;
            }
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