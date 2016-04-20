using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class AllSongsPlaylistViewModel : PlaylistItemViewModelBase
    {
        internal const string FileIdentifier = "MusicManager.AllSongsPlaylist";
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
                sw.WriteLine("#"+ FileIdentifier);
                foreach (var file in FileSystemHelper.GetFilesFromDirectory(Properties.Settings.Default.MusicPath, FileSystemHelper.SupportedMusicFileExtensions))
                {
                    var fi = new FileInfo(file);
                    if (FileSystemHelper.SupportedMusicFileExtensions.Any(x => fi.Extension.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sw.WriteLine(file);
                    }
                }
            }
        }
    }
}