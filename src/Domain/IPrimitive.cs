using WSCAD.Geometry;

namespace WSCAD.Domain 
{
	public interface IPrimitive 
	{
		PrimitiveStyle Style { get; }
		Rect GetBounds();
		void Accept(IPrimitive visitor);
	}
}