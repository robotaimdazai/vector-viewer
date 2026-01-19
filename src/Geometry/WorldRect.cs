namespace WSCAD.Geometry 
{
    public readonly record struct WorldRect(double MinX, double MinY, double MaxX, double MaxY) 
    {
        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public static WorldRect FromPoints(IEnumerable<WorldPoint> points) 
        {
            var minX = double.PositiveInfinity;
            var minY = double.PositiveInfinity;
            var maxX = double.NegativeInfinity;
            var maxY = double.NegativeInfinity;

            foreach (var p in points)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            if (double.IsInfinity(minX)) return new WorldRect(0, 0, 0, 0);

            return new WorldRect(minX, minY, maxX, maxY);
        }

        public WorldRect Inflate(double amount)
        => new WorldRect(MinX - amount, MinY - amount, MaxX + amount, MaxY + amount);
    }
}