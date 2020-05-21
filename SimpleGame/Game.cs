using System;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;

namespace SimpleGame
{
    public class Game : GameWindow
    {
        private readonly IWorld World;
        private readonly Player Player;
        private readonly TextureStorage textures;
        private readonly Renderer renderer;
        
        private DateTime previousTime;
        private MouseState previousMouseState;

        public Game(IWorld world, Player player, Renderer renderer)
        {
            World = world;
            Player = player;
            this.renderer = renderer;
            Load += OnLoad;
        }

        private void OnLoad(object? sender, EventArgs e)
        {
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            UpdateFrame += UpdateState;
            RenderFrame += OnRender;
            MouseMove += (o, args) =>
            {
                if (!CursorVisible)
                    Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            };
        }

        public void UpdateState(object sendet, FrameEventArgs args)
        {
            // Дельта сдвига мыши для движения камеры
            var state = Mouse.GetState();
            if (!CursorVisible)
            {
                Player.Yaw += state.X - previousMouseState.X;
                Player.Pitch += state.Y - previousMouseState.Y;
            }
            previousMouseState = state;

            // Для правильного движения игрока вне зависимости от частоты вызова этого метода
            var currTime = DateTime.Now;
            var delta = currTime - previousTime;
            
            previousTime = currTime;
            Player.Position += Player.AbsoluteVelocity * (float) delta.TotalSeconds;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
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
                case Key.LShift:
                case Key.RShift:
                    Player.Velocity -= Vector3.UnitY;
                    break;
                case Key.Space:
                    Player.Velocity += Vector3.UnitY;
                    break;
                case Key.P:
                    CursorVisible = !CursorVisible;
                    break;
                default:
                    Console.WriteLine($"Unknown char '{args.Key}'");
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Left:
                case Key.A:
                case Key.Right:
                case Key.D:
                    Player.Velocity = new Vector3(Player.Velocity.X, Player.Velocity.Y, 0);
                    break;
                case Key.Up:
                case Key.W:
                case Key.Down:
                case Key.S:
                    Player.Velocity = new Vector3(0, Player.Velocity.Y, Player.Velocity.Z);
                    break;
                case Key.ControlLeft:
                case Key.ControlRight:
                case Key.Space:
                    Player.Velocity = new Vector3(Player.Velocity.X, 0, Player.Velocity.Z);
                    break;
            }

        }

        private Vector2 WorldToChunkPosition(Vector2 worldPosition)
        {
            return new Vector2((int)(worldPosition.X / Chunk.Width), (int)(worldPosition.Y / Chunk.Length));
        }

        private Vector2 WorldToChunkPosition(Vector3 worldPosition)
        {
            return WorldToChunkPosition(new Vector2(worldPosition.X, worldPosition.Y));
        }

        private void OnRender(object sender, FrameEventArgs args)
        {
            var anchor = WorldToChunkPosition(Player.Position);
            renderer.Render(Player, textures, World.GetChunksInRadius(anchor, Preferences.ChunkRenderRadius));
        }
    }
}