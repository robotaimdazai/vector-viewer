using WSCAD.Geometry;

namespace WSCAD.Domain 
{
    public sealed class LinePrimitive : IPrimitive
    {
        public LinePrimitive(WorldPoint a, WorldPoint b, PrimitiveStyle style)
        {
            A = a; B = b; Style = style;
        }

        public WorldPoint A { get; }
        public WorldPoint B { get; }
        public PrimitiveStyle Style{get;}

        public WorldRect GetBounds() => WorldRect.FromPoints(new[] { A, B }).Inflate(Style.StrokeWidthWorld);
    }
}