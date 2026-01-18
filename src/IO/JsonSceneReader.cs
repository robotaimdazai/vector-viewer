
using System.Globalization;
using System.IO;
using System.Text.Json;
using WSCAD.Domain;
using WSCAD.Geometry;

namespace WSCAD.IO 
{
    public sealed class JsonSceneReader : ISceneReader
    {
        public string FormatName => "JSON";
        private readonly Dictionary<string, Func<JsonElement, IPrimitive>> _registry;

        public JsonSceneReader() 
        {
            _registry = new Dictionary<string, Func<JsonElement, IPrimitive>>
            (StringComparer.OrdinalIgnoreCase)
            {
                ["line"] = ParseLine,
                ["circle"] = ParseCircle,
                ["triangle"] = ParseTriangle
            };
                
        }

        public async Task<Scene> ReadAsync(Stream stream, CancellationToken ct)
        {
            using var doc = 
                await JsonDocument.ParseAsync(stream,cancellationToken:ct).ConfigureAwait(false);

            var root = doc.RootElement;
            var list= new List<IPrimitive>();
            foreach (var obj in root.EnumerateArray()) 
            {
                var type = obj.GetProperty("type").GetString()!;
                var create = _registry[type];
                list.Add(create(obj));
            }
            return new Scene(list);
        }

        //parse primitives

        private IPrimitive ParseLine(JsonElement obj) 
        {
            var a = ParsePoint(obj.GetProperty("a").GetString()!);
            var b = ParsePoint(obj.GetProperty("b").GetString()!);
            var style = ParseStyle(obj);

            return new LinePrimitive(a, b, style);
        }

        private IPrimitive ParseCircle(JsonElement obj) 
        {
            var center = ParsePoint(obj.GetProperty("center").GetString()!);
            var radius = obj.GetProperty("radius").GetDouble();
            var style = ParseStyle(obj);
            return new CirclePrimitive(center, radius, style);
        }

        private IPrimitive ParseTriangle(JsonElement obj)
        {
            var a = ParsePoint(obj.GetProperty("a").GetString()!);
            var b = ParsePoint(obj.GetProperty("b").GetString()!);
            var c = ParsePoint(obj.GetProperty("c").GetString()!);
            var style = ParseStyle(obj);
            return new TrianglePrimitive(a, b, c, style);
        }

        //parse utils
        private static double ParseDoubleLoose(string token) 
        {
            token = token.Trim().Replace(',', '.');
            return double.Parse(token, CultureInfo.InvariantCulture);
        }

        private static Point ParsePoint(String s) 
        {
            var parts = s.Split(';');
            return new Point(ParseDoubleLoose(parts[0]), ParseDoubleLoose(parts[1]));
        }

        private static PrimitiveStyle ParseStyle(JsonElement obj, bool fillDefault = false) 
        {
            var (A, R, G, B) = ParseArgb(obj.GetProperty("color").GetString()!);
            var filled = fillDefault;
            if (obj.TryGetProperty("filled", out var f))
            {
                filled = f.GetBoolean();
            }
            return new PrimitiveStyle(A, R, G, B, filled, StrokeWidthWorld: 1.0);

        }

        private static (byte A, byte R, byte G, byte B) ParseArgb(string s) 
        {
            var parts = s.Split(';').Select(p=>p.Trim()).ToArray();
            return 
            (
                byte.Parse(parts[0]), 
                byte.Parse(parts[1]),
                byte.Parse(parts[2]), 
                byte.Parse(parts[3])
            );
        }

    }
}