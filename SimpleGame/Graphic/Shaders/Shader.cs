using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Shaders
{
    public abstract class Shader : IStaticShader
    {
        public int ProgramId { get; private set; }
        public bool IsActive { get; private set; }

        protected Dictionary<ShaderType, string> ShadersFilenames = new Dictionary<ShaderType, string>();
        
        private List<int> ShadersIds = new List<int>();
        private int usingCounter;

        public Shader Start()
        {
            usingCounter++;
            if (ProgramId == 0)
                throw new Exception("Program is delete or has errors");
            GL.UseProgram(ProgramId);
            IsActive = true;
            return this;
        }

        public void Dispose()
        {
            usingCounter--;
            if (usingCounter > 0)
                return;
            GL.UseProgram(0);
            IsActive = false;
        }

        public void Remove()
        {
            GL.DeleteProgram(ProgramId);
            ProgramId = 0;
        }

        public abstract void BindAttributes();
        public abstract void BindUniformVariables();

        public void Initialize()
        {
            LoadShaders();
            ProgramId = GL.CreateProgram();
            AttachShaders();
            BindAttributes();
            GL.LinkProgram(ProgramId);
            Console.Error.WriteLine($"Linking program. Errors:\n{GL.GetProgramInfoLog(ProgramId)}");
            DeleteShaders();
            BindUniformVariables();
        }

        protected void LoadShaders()
        {
            foreach (var shader in ShadersFilenames)
            {
                var id = GL.CreateShader(shader.Key);
                GL.ShaderSource(id, File.ReadAllText(shader.Value));
                GL.CompileShader(id);
                var errors = GL.GetShaderInfoLog(id);
                Console.Error.WriteLine($"Loading {shader.Key}\nfrom file: {shader.Value}\nErrors: {(errors == "" ? "No errors" : errors)}\n");

                ShadersIds.Add(id);
            }
        }

        protected void BindAttribute(int attribute, string attributeName)
        {
            GL.BindAttribLocation(ProgramId, attribute, attributeName);
        }

        protected int BindUniformVariable(string variableName)
        {
            return GL.GetUniformLocation(ProgramId, variableName);
        }

        protected void LoadMatrix4(int uniformId, Matrix4 matrix)
        {
            var shouldStop = !IsActive;
            if (shouldStop)
                Start();
            GL.UniformMatrix4(uniformId, false, ref matrix);
            if (shouldStop)
                Dispose();
        }

        protected void LoadFloat(int uniformId, float value)
        {
            using (Start())
            {
                GL.Uniform1(uniformId, value);
            }
        }

        private void AttachShaders()
        {
            foreach (var shader in ShadersIds)
            {
                GL.AttachShader(ProgramId, shader);
            }
        }

        private void DeleteShaders()
        {
            foreach (var shader in ShadersIds)
            {
                GL.DeleteShader(shader);
            }
        }
        
        public abstract Matrix4 ViewMatrix { set; }
        public abstract Matrix4 ProjectionMatrix { set; }

        public abstract Matrix4 TransformationMatrix  { set; }
        
        public abstract bool IsTextured { set; }
    }
}