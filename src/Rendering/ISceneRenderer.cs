using System.Windows.Media;
using WSCAD.Domain;

namespace WSCAD.Rendering
{
    public interface ISceneRenderer
    {
        void Render(Scene scene, DrawingContext drawingContext, ViewTransform viewTransform);
    }
}

