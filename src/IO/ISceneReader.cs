using System.IO;
using WSCAD.Domain;

namespace WSCAD.IO 
{
    public interface ISceneReader 
    {
        Task<Scene> ReadAsync(Stream stream, CancellationToken ct);
        string FormatName { get; }
    }
}