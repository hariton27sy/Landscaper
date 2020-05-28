using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimpleGame.Graphic.Shaders
{
    public class StaticShader : Shader
    {
        private const string VertexShaderFilename = "Graphic/Shaders/colored.vert";
        private const string FragmentShader = "Graphic/Shaders/simple.frag";

        private int viewMatrix, projectionMatrix, transformationMatrix;
        private int isTextured;
        
        public override void SetViewMatrix(Matrix4 matrix)
        {
            LoadMatrix4(viewMatrix, matrix);
        }

        public override void SetProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix4(projectionMatrix, matrix);
        }

        public override void SetTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix4(transformationMatrix, matrix);
        }

        public override void SetIsTextured(bool value)
        {
            var number = value ? 1f : 0;
            LoadFloat(isTextured, number);
        }
        
        public StaticShader()
        {
            shadersFilenames.Add(ShaderType.VertexShader, VertexShaderFilename);
            shadersFilenames.Add(ShaderType.FragmentShader, FragmentShader);
        }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "in_color");
            BindAttribute(2, "tex_coords");
        }

        public override void BindUniformVariables()
        {
            transformationMatrix = BindUniformVariable("transformationMatrix");
            projectionMatrix = BindUniformVariable("projectionMatrix");
            viewMatrix = BindUniformVariable("viewMatrix");
            isTextured = BindUniformVariable("isTextured");
        }

        public override void LoadShaders()
        
        {
            foreach (var shader in shadersFilenames)
            {
                var id = GL.CreateShader(shader.Key);
                GL.ShaderSource(id, File.ReadAllText(shader.Value));
                GL.CompileShader(id);
                var errors = GL.GetShaderInfoLog(id);
                Console.Error.WriteLine($"Loading {shader.Key}\nfrom file: {shader.Value}\nErrors: {(errors == "" ? "No errors" : errors)}\n");

                shadersIds.Add(id);
            }
        }
    }
}