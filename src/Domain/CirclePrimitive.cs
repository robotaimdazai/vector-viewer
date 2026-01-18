using WSCAD.Geometry;

namespace WSCAD.Domain 
{
    public sealed class CirclePrimitive : IPrimitive
    {
        public CirclePrimitive(Point center, double radius, PrimitiveStyle style) 
        {
            Center = center; Radius = radius; Style = style;
        }

        public Point Center { get; }
        public double Radius { get; }
        public PrimitiveStyle Style { get; } 


        public Rect GetBounds()=> new Rect(
            Center.X - Radius, Center.Y - Radius,
            Center.X + Radius, Center.Y + Radius
        ).Inflate(Style.StrokeWidthWorld);

        public void Accept(IPrimitive visitor)
        {

        }

    }
}