using WSCAD.Geometry;

namespace WSCAD.Domain 
{
	public interface IPrimitive 
	{
		PrimitiveStyle Style { get; }
		WorldRect GetBounds();
	}
}