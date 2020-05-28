using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using SimpleGame.Graphic.Models;

namespace SimpleGame.Graphic
{
    public interface IRenderer
    {
        void SetAspect(float aspect);
        void Start();
        void Clear(Color color=default);
        void Render(ICamera camera, ITextureStorage storage, IEnumerable<IEntity> entities);
    }
}