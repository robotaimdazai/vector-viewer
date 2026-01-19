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
            var baseScale = Math.Min(fitScale, 1.0); // At 100% zoom: 1 world unit == 1 pixel (no upscaling)
            Scale = baseScale * Zoom;

            // Center the scene within the available viewport.
            OffsetXPx = MarginPx + (availW - worldW * Scale) / 2.0;
            OffsetYPx = MarginPx + (availH - worldH * Scale) / 2.0;
        }

        public WorldRect WorldBounds { get; }
        public double ViewportWidthPx { get; }
        public double ViewportHeightPx { get; }
        public double MarginPx { get; }
        public double Zoom { get; }
        public double Scale { get; }
        public double OffsetXPx { get; }
        public double OffsetYPx { get; }

        public Point ToScreen(WorldPoint w)
        {
            // X: left to right
            var x = OffsetXPx + (w.X - WorldBounds.MinX) * Scale;
            // Y: flip (world Y up) -> (screen Y down)
            var y = OffsetYPx + (WorldBounds.MaxY - w.Y) * Scale;
            return new Point(x, y);
        }

        public double ToPixels(double worldLength) => worldLength * Scale;
    }
}
