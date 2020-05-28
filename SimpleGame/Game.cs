using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore;
using SimpleGame.GameCore.Persons;
using SimpleGame.GameCore.Worlds;
using SimpleGame.Graphic;
using SimpleGame.Graphic.Models;
using SimpleGame.Graphic.Shaders;

namespace SimpleGame
{
    public class Game : GameWindow
    {
        private readonly IWorld World;
        private readonly IPlayer Player;
        private readonly ITextureStorage textures;
        private readonly IRenderer renderer;
        
        private DateTime previousTime;
        private MouseState previousMouseState;

        public Game(IWorld world, IPlayer player, ITextureStorage textures, IRenderer renderer)
        {
            World = world;
            Player = player;
            this.textures = textures;
            this.renderer = renderer;
            Load += OnLoad;
            VSync = VSyncMode.On;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            textures.LoadTextures();
            renderer.Start();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            UpdateFrame += UpdateState;
            RenderFrame += OnRender;
            MouseMove += (o, args) =>
            {
                if (!CursorVisible)
                    Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            };
            renderer.Aspect = (float) Width / Height;
            previousTime = DateTime.Now;
        }

        public void UpdateState(object sendet, FrameEventArgs args)
        {
            // Дельта сдвига мыши для движения камеры
            var state = Mouse.GetState();
            if (!CursorVisible)
            {
                Player.Yaw += state.X - previousMouseState.X;
                Player.Pitch -= state.Y - previousMouseState.Y;
            }
            previousMouseState = state;

            // Для правильного движения игрока вне зависимости от частоты вызова этого метода
            var currTime = DateTime.Now;
            var delta = currTime - previousTime;

            previousTime = currTime;
            World.Update(delta);
            Title = $"{Player.Position.X} {Player.Position.Y} {Player.Position.Z}";
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs args)
        {
            var speed = 20;
            switch (args.Key)
            {
                case Key.Left:
                case Key.A:
                    Player.Velocity = new Vector3(Player.Velocity.X, Player.Velocity.Y, -speed);
                    break;
                case Key.Right:
                case Key.D:
                    Player.Velocity = new Vector3(Player.Velocity.X, Player.Velocity.Y, speed);
                    break;
                case Key.Up:
                case Key.W:
                    Player.Velocity = new Vector3(speed, Player.Velocity.Y, Player.Velocity.Z);
                    break;
                case Key.Down:
                case Key.S:
                    Player.Velocity = new Vector3(-speed, Player.Velocity.Y, Player.Velocity.Z);
                    break;
                case Key.LShift:
                case Key.RShift:
                    Player.Velocity = new Vector3(Player.Velocity.X, -speed, Player.Velocity.Z);
                    break;
                case Key.Space:
                    Player.Velocity = new Vector3(Player.Velocity.X, speed, Player.Velocity.Z);
                    break;
                case Key.Escape:
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
                case Key.LShift:
                case Key.RShift:
                case Key.Space:
                    Player.Velocity = new Vector3(Player.Velocity.X, 0, Player.Velocity.Z);
                    break;
            }

        }

        private void OnRender(object sender, FrameEventArgs args)
        {
            var anchor = Player.Position.ToChunkPosition();
            renderer.Clear(Color.Aqua);
            var entities = World.GetChunksInRadius(anchor, Preferences.ChunkRenderRadius);
            renderer.Render(Player, textures, entities);

            SwapBuffers();
        }
    }
}