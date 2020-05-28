using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Shaders
{
    public abstract class Shader : IStaticShader
    {
        public int GetProgramId()
        {
            return programId;
        }
        
        private void SetProgramId(int id)
        {
            programId = id;
        }


        public abstract void SetIsTextured(bool isTextured);

        public bool IsActive { get; private set; }
        
        private int usingCounter;
        
        protected readonly Dictionary<ShaderType, string> shadersFilenames = new Dictionary<ShaderType, string>();
        
        protected readonly List<int> shadersIds = new List<int>();
        private int programId;

        public IStaticShader Start()
        {
            usingCounter++;
            if (programId == 0)
                throw new Exception("Program is delete or has errors");
            GL.UseProgram(programId);
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
            GL.DeleteProgram(programId);
            programId = 0;
        }

        public abstract void BindAttributes();
        public abstract void BindUniformVariables();

        public void Initialize()
        {
            LoadShaders();
            SetProgramId(GL.CreateProgram());
            AttachShaders();
            BindAttributes();
            GL.LinkProgram(programId);
            Console.Error.WriteLine($"Linking program. Errors:\n{GL.GetProgramInfoLog(programId)}");
            DeleteShaders();
            BindUniformVariables();
        }
        public abstract void LoadShaders();

        protected void BindAttribute(int attribute, string attributeName)
        {
            GL.BindAttribLocation(programId, attribute, attributeName);
        }

        protected int BindUniformVariable(string variableName)
        {
            return GL.GetUniformLocation(programId, variableName);
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

        public void AttachShaders()
        {
            foreach (var shader in shadersIds)
            {
                GL.AttachShader(programId, shader);
            }
        }

        public void DeleteShaders()
        {
            foreach (var shader in shadersIds)
            {
                GL.DeleteShader(shader);
            }
        }

        public abstract void SetViewMatrix(Matrix4 matrix);
        public abstract void SetProjectionMatrix(Matrix4 matrix);
        public abstract void SetTransformationMatrix(Matrix4 matrix);
    }
}