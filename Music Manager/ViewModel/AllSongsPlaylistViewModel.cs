using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class AllSongsPlaylistViewModel : PlaylistItemViewModelBase
    {
        public AllSongsPlaylistViewModel()
        {
            this.Name = "All";
            this.UpdateSongsCommand = new RelayCommand(this.Save);
        }

        public ICommand UpdateSongsCommand { get; }

        public override void Save()
        {
            var path = this.GetPlaylistfilePath();
            var folder = Directory.GetParent(path).FullName;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (var sw = new StreamWriter(new FileStream(path, FileMode.Create)))
            {
                sw.WriteLine("#MusicManager.AllSongsPlaylist");
                foreach (var file in this.GetMusicFiles(Properties.Settings.Default.MusicPath))
                {
                    var fi = new FileInfo(file);
                    if (MusicFileHelper.SupportedFileExtensions.Any(x => fi.Extension.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sw.WriteLine(file);
                    }
                }
            }
        }

        private IEnumerable<string> GetMusicFiles(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                var fi = new FileInfo(file);
                if (MusicFileHelper.SupportedFileExtensions.Any(x => fi.Extension.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return file;
                }
            }

            foreach (var file in Directory.GetDirectories(folder).SelectMany(this.GetMusicFiles))
            {
                yield return file;
            }
        }
    }
}