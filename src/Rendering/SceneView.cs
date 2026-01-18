using System.Windows;
using System.Windows.Media;

namespace WSCAD.Rendering
{
    public sealed class SceneView : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(0, 0, ActualWidth, ActualHeight));
        }
    }
}
