using System;

namespace SimpleGame.Graphic.Models
{
    public interface ITextureStorage : IDisposable
    {
        Texture this[int value] { get; }
        Texture this[string value] { get; }
        void LoadTextures();
    }
}