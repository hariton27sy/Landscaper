using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using SimpleGame.GameCore.GameModels;
using SimpleGame.GraphicEngine.Shaders;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.GraphicEngine
{
    public class TestWindow : GameWindow
    {
        private Loader loader;
        private Renderer renderer;
        private Camera camera;

        private bool isMouseFixed;
        private MouseState previousState;

        private Entity entity;
        private Entity entity2;
        private ModelsStorage modelsStorage;

        public TestWindow()
        {
            Load += OnLoad;
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
            VSync = VSyncMode.On;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            const float sensitivity = 0.1f;
            Vector3 delta = Vector3.Zero;
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
                    Console.WriteLine(e.KeyChar);
                    break;
            }
            if (delta != Vector3.Zero)
                Console.WriteLine(delta);
            camera.MoveLocalByDelta(delta);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            CheckMouse();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            renderer.Clear(Color.Aqua);
            renderer.Render(entity);
            renderer.Render(entity2);
            SwapBuffers();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            Console.WriteLine(GL.GetString(StringName.Version));
            loader = new Loader();
            modelsStorage = new ModelsStorage(loader, "textures");
            entity = new Entity(modelsStorage[0], new Vector3(0, 0, 0));
            entity2 = new Entity(modelsStorage[2], new Vector3(1, 0, 0), scale:0.5f);
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