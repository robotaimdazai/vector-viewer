using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WSCAD.Domain;

namespace WSCAD.Rendering
{
    public sealed class SceneView : FrameworkElement
    {
        private static readonly ISceneRenderer _renderer = new WpfSceneRenderer();

        public static readonly DependencyProperty SceneProperty =
            DependencyProperty.Register
            (
                nameof(Scene),
                typeof (Scene),
                typeof(SceneView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register
            (
                nameof(Zoom),
                typeof(double),
                typeof(SceneView),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender)
            );


        public Scene? Scene 
        { 
            get => (Scene?)GetValue(SceneProperty);
            set => SetValue(SceneProperty, value);
        }
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.WhiteSmoke, null, new Rect(0, 0, ActualWidth, ActualHeight));

            if (Scene is null) 
            {
                var text = new FormattedText
                (
                    "Open a JSON file to view vectors",
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Segoe UI"),
                    16,
                    Brushes.DimGray,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip
                );
                drawingContext.DrawText(text, new Point(20, 20));
                return;
            }

            var vt = new ViewTransform(Scene.Bounds, ActualWidth,
                ActualHeight, zoom: Zoom, marginPx: 20);

             _renderer.Render(Scene, drawingContext, vt);
        }
    }
}
