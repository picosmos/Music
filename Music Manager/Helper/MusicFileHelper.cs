using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koopakiller.Apps.MusicManager.Helper
{
    public class MusicFileHelper
    {
        public static IReadOnlyCollection<string> SupportedFileExtensions => new[] { ".mp3", ".wav", ".m4a", ".flac" };
    }
}
