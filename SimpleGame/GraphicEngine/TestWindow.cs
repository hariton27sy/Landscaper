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

namespace SimpleGame.GraphicEngine
{
    public class TestWindow : GameWindow
    {
        private Loader loader;
        private Renderer renderer;
        private Camera camera;

        private bool isMouseFixed;
        private MouseState previousState;

        private List<Entity> entities;
        private ModelsStorage modelsStorage;

        private readonly Game game;

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

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            const float sensitivity = 0.1f;
            Vector3 delta = Vector3.Zero;
            
            // todo to Key.Q...
            switch (e.KeyChar)
            {
                case 'q': camera.Yaw -= 1f;
                    break;
                case 'e': camera.Yaw += 1f;
                    break;
                case 'w': delta += Vector3.UnitX * sensitivity;
                    break;
                case 'a':
                    delta -= Vector3.UnitZ * sensitivity;
                    break;
                case 's':
                    delta -= Vector3.UnitX * sensitivity;
                    break;
                case 'd':
                    delta += Vector3.UnitZ * sensitivity;
                    break;
                case 'p':
                    //Fix mouse pointer
                    CursorVisible = isMouseFixed;
                    isMouseFixed = !isMouseFixed;
                    previousState = Mouse.GetState();
                    break;
                default:
                    Console.WriteLine($"Unknown char '{e.KeyChar}'");
                    break;
            }
            if (delta != Vector3.Zero)
                Console.WriteLine(delta);
            camera.MoveLocalByDelta(delta);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            entities.Clear();
            foreach (var tuple in game.GetEntitiesToRender())
            {
                var mapEntity = tuple.Item1;
                var location = tuple.Item2;
                var textureId = modelsStorage.NameToId[mapEntity.TextureName];
                var entity = new Entity(modelsStorage[textureId], location);
                entities.Add(entity);
            }
            CheckMouse();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            renderer.Clear(Color.Aqua);

            foreach (var entity in entities)
            {
                renderer.Render(entity);
            }
            SwapBuffers();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            loader = new Loader();
            modelsStorage = new ModelsStorage(loader, "textures");
            entities = new List<Entity>();
            camera = new Camera();
            renderer = new Renderer(camera, new StaticShader(), (float)Width / Height);
            
            
            KeyPress += OnKeyPress;
        }

        private void CheckMouse()
        {
            const float sensibility = 1f;
            var mouseState = Mouse.GetState();
            var deltaX = mouseState.X - previousState.X;
            var deltaY = mouseState.Y - previousState.Y;
            previousState = mouseState;
            if (!isMouseFixed)
                return;

            camera.Pitch -= deltaY * sensibility;
            camera.Yaw += deltaX * sensibility;
            Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
        }

        private void DebugPrint()
        {
        }
    }
}