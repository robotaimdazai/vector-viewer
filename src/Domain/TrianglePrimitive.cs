using WSCAD.Geometry;

namespace WSCAD.Domain
{
    public sealed class TrianglePrimitive : IPrimitive
    {
        public TrianglePrimitive(Point a, Point b, Point c, PrimitiveStyle style)
        {
            A = a; B = b; C = c; Style = style;
        }

        public Point A { get; }
        public Point B { get; }
        public Point C { get; }
        public PrimitiveStyle Style { get; }

        public Rect GetBounds() =>  Rect.FromPoints(new[] { A, B, C }).Inflate(Style.StrokeWidthWorld);

        public void Accept(IPrimitive visitor)
        {

        }
    }
}