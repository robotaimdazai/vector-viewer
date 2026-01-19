using WSCAD.Geometry;

namespace WSCAD.Domain 
{
    public sealed class CirclePrimitive : IPrimitive
    {
        public CirclePrimitive(WorldPoint center, double radius, PrimitiveStyle style) 
        {
            Center = center; Radius = radius; Style = style;
        }

        public WorldPoint Center { get; }
        public double Radius { get; }
        public PrimitiveStyle Style { get; } 


        public WorldRect GetBounds()=> new WorldRect(
            Center.X - Radius, Center.Y - Radius,
            Center.X + Radius, Center.Y + Radius
        ).Inflate(Style.StrokeWidthWorld);

        public void Accept(IPrimitive visitor)
        {

        }

    }
}