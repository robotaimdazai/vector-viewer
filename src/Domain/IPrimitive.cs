using WSCAD.Geometry;

namespace WSCAD.Domain 
{
	public interface IPrimitive 
	{
		PrimitiveStyle Style { get; }
		WorldRect GetBounds();
		void Accept(IPrimitive visitor); // this is for accepting clicks to display info in future
	}
}