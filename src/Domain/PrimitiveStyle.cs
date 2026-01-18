namespace WSCAD.Domain 
{
    public sealed record PrimitiveStyle(byte A, byte R, byte G, byte B,
        bool Filled, double StrokeWidthWorld = 1.0)
    {
        public uint Argb => (uint)( (A<<24) | (R<<16) | (G<<8) | B );
    }
}