using System.Windows;
using WSCAD.Geometry;

namespace WSCAD.Rendering 
{
    public sealed class ViewTransform
    {
        public ViewTransform(WorldRect worldBounds, double viewportWidthPx, 
            double viewportHeightPx, double zoom = 1.0, double marginPx = 20.0)
        {
            WorldBounds = worldBounds;
            ViewportWidthPx = viewportWidthPx;
            ViewportHeightPx = viewportHeightPx;
            Zoom = zoom <= 0 ? 1.0 : zoom;
            MarginPx = marginPx;

            var availW = Math.Max(1.0, ViewportWidthPx - 2 * MarginPx);
            var availH = Math.Max(1.0, ViewportHeightPx - 2 * MarginPx);

            var worldW = Math.Max(1e-9, worldBounds.Width);
            var worldH = Math.Max(1e-9, worldBounds.Height);

            var fitScale = Math.Min(availW / worldW, availH / worldH);

            Scale = fitScale * Zoom;
        }

        public WorldRect WorldBounds { get; }
        public double ViewportWidthPx { get; }
        public double ViewportHeightPx { get; }
        public double MarginPx { get; }
        public double Zoom { get; }
        public double Scale { get; }

        public Point ToScreen(WorldPoint w)
        {
            // X: left to right
            var x = MarginPx + (w.X - WorldBounds.MinX) * Scale;
            // Y: flip (world Y up) -> (screen Y down)
            var y = MarginPx + (WorldBounds.MaxY - w.Y) * Scale;
            return new Point(x, y);
        }

        public double ToPixels(double worldLength) => worldLength * Scale;
    }
}
