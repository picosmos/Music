using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Koopakiller.Apps.MusicManager.Helper
{
    public class PathBuilder
    {
        public PathBuilder()
        {
            this.Replacements = new Dictionary<string, string>();
        }


        public string Build()
        {
            var parts = Regex.Matches(this.Pattern, @"\{(\w*|)*\w*\}");

            var sb = new StringBuilder();
            var last = 0;
            foreach (Match part in parts)
            {
                sb.Append(this.Pattern.Substring(last, part.Index - last));
                last = part.Index + part.Length;

                var x = part.Value.Split('|').Select(s => this.Replacements[s]).FirstOrDefault(val => val != null);
                if (x == null)
                {
                    throw new NullReferenceException();
                }
                sb.Append(x);
            }
            sb.Append(this.Pattern.Substring(last));

            return sb.ToString();
        }

        public string Pattern { get; set; }

        public Dictionary<string, string> Replacements { get; }

        public string RootPath { get; set; }
    }
}
