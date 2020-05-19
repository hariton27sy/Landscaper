using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GraphicEngine.Shaders;
using OpenTK.Graphics.OpenGL;
using SimpleGame.GameCore;
using SimpleGame.GameCore.Worlds;

namespace SimpleGame.GraphicEngine
{
    public class TestWindow : GameWindow
    {
        private Loader loader;
        private Renderer renderer;
        private Camera camera;

        private bool isMouseFixed;
        private MouseState previousState;

        private ModelsStorage modelsStorage;

        private Game game;

        public TestWindow()
        {
            Load += OnLoad;
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            VSync = VSyncMode.On;
        }

        public TestWindow(Game game) : this()
        {
            this.game = game;
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            CheckMouse();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            renderer.Clear(Color.Aqua);

            renderer.Render(game.OnRender());
            SwapBuffers();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            
            loader = new Loader();
            modelsStorage = new ModelsStorage(loader, "textures");
            
            camera = new Player();
            var world = new TestWorld(modelsStorage);
            game = new Game(world);
            renderer = new Renderer(camera, new StaticShader(), (float)Width / Height);

            KeyDown += game.OnKeyDown;
            KeyUp += game.OnKeyUp;
        }

        private void CheckMouse()
        {
            var mouseState = Mouse.GetState();
            var deltaX = mouseState.X - previousState.X;
            var deltaY = mouseState.Y - previousState.Y;
            previousState = mouseState;
            if (!isMouseFixed)
                return;

            game.OnMouse(new MouseArgs(deltaX, deltaY, mouseState.LeftButton == ButtonState.Pressed, 
                mouseState.RightButton == ButtonState.Pressed));
        }
    }
}