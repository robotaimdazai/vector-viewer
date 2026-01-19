using System.Windows.Media;
using WSCAD.Domain;

namespace WSCAD.Rendering
{
    public sealed class WpfSceneRenderer : ISceneRenderer
    {
        public void Render(Scene scene, DrawingContext drawingContext, ViewTransform viewTransform)
        {
            foreach (var primitive in scene.Primitives)
            {
                DrawPrimitive(drawingContext, primitive, viewTransform);
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
                fill = new SolidColorBrush(color);
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

