using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Koopakiller.Apps.MusicManager.Helper
{
    public static class FileSystemHelper
    {
        public static IReadOnlyCollection<string> SupportedMusicFileExtensions => new[] { ".mp3", ".wav", ".m4a", ".flac" };

        public static IEnumerable<string> GetFilesFromDirectory(string folder, IReadOnlyCollection<string> extensions)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                var fi = new FileInfo(file);
                if (extensions.Any(x => fi.Extension.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return file;
                }
            }

            foreach (var file in Directory.GetDirectories(folder).SelectMany(x => GetFilesFromDirectory(x, extensions)))
            {
                yield return file;
            }
        }
    }
}
