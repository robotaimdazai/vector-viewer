using WSCAD.Geometry;

namespace WSCAD.Domain 
{
    public sealed class LinePrimitive : IPrimitive
    {
        public LinePrimitive(Point a, Point b, PrimitiveStyle style)
        {
            A = a; B = b; Style = style;
        }

        public Point A { get; }
        public Point B { get; }
        public PrimitiveStyle Style{get;}

        public Rect GetBounds() => Rect.FromPoints(new[] { A, B }).Inflate(Style.StrokeWidthWorld);

        public void Accept(IPrimitive visitor)
        {
            
        }

        
    }
}