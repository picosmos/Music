using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Koopakiller.Apps.MusicManager.Helper
{
    public class PathBuilder
    {
        private string _pattern;
        private string _rootPath;

        public PathBuilder(string rootpath, string pattern)
        {
            this.Replacements = new Dictionary<string, string>();
            this.RootPath = rootpath;
            this.Pattern = pattern;
        }

        public string Build()
        {
            var parts = Regex.Matches(this.Pattern, @"<(\w+\|)*\w+>");

            var sb = new StringBuilder();
            var last = 0;
            foreach (Match part in parts)
            {
                sb.Append(this.Pattern.Substring(last, part.Index - last));
                last = part.Index + part.Length;

                var x = part.Value.Substring(1, part.Length - 2)
                                  .Split('|')
                                  .Select(s => this.Replacements[s])
                                  .FirstOrDefault(val => val != null);
                if (x == null)
                {
                    throw new NullReferenceException();
                }
                sb.Append(x);
            }
            sb.Append(this.Pattern.Substring(last));
            
            return Path.Combine(this.RootPath, sb.ToString());
        }
        
        public string Pattern
        {
            get { return this._pattern; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this._pattern = value;
            }
        }

        public Dictionary<string, string> Replacements { get; }

        public string RootPath
        {
            get { return this._rootPath; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this._rootPath = value;
            }
        }
    }
}
