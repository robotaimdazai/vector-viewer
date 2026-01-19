using WSCAD.Geometry;

namespace WSCAD.Domain
{
    public sealed class TrianglePrimitive : IPrimitive
    {
        public TrianglePrimitive(WorldPoint a, WorldPoint b, WorldPoint c, PrimitiveStyle style)
        {
            A = a; B = b; C = c; Style = style;
        }

        public WorldPoint A { get; }
        public WorldPoint B { get; }
        public WorldPoint C { get; }
        public PrimitiveStyle Style { get; }

        public WorldRect GetBounds() =>  WorldRect.FromPoints(new[] { A, B, C }).Inflate(Style.StrokeWidthWorld);
    }
}