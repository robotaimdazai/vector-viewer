using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WSCAD.Domain;

namespace WSCAD.Rendering
{
    public sealed class SceneView : FrameworkElement
    {
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

            foreach (var p in Scene.Primitives)
            {
                DrawPrimitive(drawingContext, p, vt);
            }
        }

        private static void DrawPrimitive(DrawingContext dc, IPrimitive p, ViewTransform vt)
        {
            var style = p.Style;
            var color = Color.FromArgb(style.A, style.R, style.G, style.B);

            var stroke = new SolidColorBrush(color);
            stroke.Freeze();

            Brush? fill = null;
            if (style.Filled)
            {
                fill = new SolidColorBrush(
                    Color.FromArgb( style.A, style.R, style.G, style.B));
                fill.Freeze();
            }

            var pen = new Pen(stroke, Math.Max(1.0, vt.ToPixels(style.StrokeWidthWorld)));
            pen.Freeze();

            switch (p)
            {
                case LinePrimitive line:
                    dc.DrawLine(pen, vt.ToScreen(line.A), vt.ToScreen(line.B));
                    break;

                case CirclePrimitive circle:
                    {
                        var c = vt.ToScreen(circle.Center);
                        var r = vt.ToPixels(circle.Radius);
                        dc.DrawEllipse(fill, pen, c, r, r);
                        break;
                    }

                case TrianglePrimitive tri:
                    {
                        var g = new StreamGeometry();
                        using (var ctx = g.Open())
                        {
                            ctx.BeginFigure(vt.ToScreen(tri.A), isFilled: style.Filled, isClosed: true);
                            ctx.LineTo(vt.ToScreen(tri.B), isStroked: true, isSmoothJoin: false);
                            ctx.LineTo(vt.ToScreen(tri.C), isStroked: true, isSmoothJoin: false);
                        }
                        g.Freeze();
                        dc.DrawGeometry(fill, pen, g);
                        break;
                    }
            }
        }
    }
}
