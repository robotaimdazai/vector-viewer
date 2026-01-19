using WSCAD.Geometry;

namespace WSCAD.Domain 
{
    public sealed class Scene 
    {
        public Scene(IReadOnlyList<IPrimitive> primitives) 
        {
            Primitives = primitives;
            Bounds = ComputeBounds(primitives);
        }

        public IReadOnlyList<IPrimitive> Primitives { get; }
        public WorldRect Bounds { get; }

        private static WorldRect ComputeBounds(IReadOnlyList<IPrimitive> primitives) 
        {
            if (primitives.Count == 0) return new WorldRect(0, 0, 0, 0);
            var rect = primitives[0].GetBounds();
            for (var i = 1; i < primitives.Count; i++)
            {
                var b = primitives[i].GetBounds();
                rect = new WorldRect(
                    Math.Min(rect.MinX, b.MinX),
                    Math.Min(rect.MinY, b.MinY),
                    Math.Max(rect.MaxX, b.MaxX),
                    Math.Max(rect.MaxY, b.MaxY)
                );
            }
            return rect;
        }
    }
}