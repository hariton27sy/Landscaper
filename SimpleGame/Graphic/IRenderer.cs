using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IRenderer
    {
        Matrix4 ProjectionMatrix { get; set; }
        float Aspect { set; }
        void Start();
        void Clear(Color color=default);
        void Render(ICamera camera, ITextureStorage storage, IEnumerable<IEntity> entities);
    }
}