using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class PlaylistsViewModel : HeaderViewModelBase
    {
        public PlaylistsViewModel() : base("Playlists")
        {
            this.Playlists = new ObservableCollection<PlaylistItemViewModelBase>();
            this.Playlists.CollectionChanged += this.OnPlaylistsChanged;

            this.AddAllSongsPlaylistCommand = new RelayCommand(this.OnAddAllSongsPlaylist, () => !this.AllSongsPlaylistExists);
        }

        private void OnPlaylistsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ((RelayCommand)this.AddAllSongsPlaylistCommand).RaiseCanExecuteChanged();
        }

        public ObservableCollection<PlaylistItemViewModelBase> Playlists { get; }

        private bool AllSongsPlaylistExists => this.Playlists.OfType<AllSongsPlaylistViewModel>().Any();

        public ICommand AddAllSongsPlaylistCommand { get; }

        private void OnAddAllSongsPlaylist()
        {
            var pl = new AllSongsPlaylistViewModel();
            this.Playlists.Add(pl);
            pl.Save();
        }
    }

    public abstract class PlaylistItemViewModelBase : ViewModelBase
    {
        private string _name;

        protected PlaylistItemViewModelBase()
        {
            this.OpenCommand = new RelayCommand(this.OnOpen);
            this.OpenFolderCommand = new RelayCommand(this.OnOpenFolder);
        }

        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public string FileExtension => ".m3u";

        protected string GetPlaylistfilePath()
        {
            return Path.Combine(Properties.Settings.Default.PlaylistsPath, this.Name + this.FileExtension);
        }

        public abstract void Save();

        public ICommand OpenFolderCommand { get; }
        public ICommand OpenCommand { get; }


        private void OnOpenFolder()
        {
            var path = this.GetPlaylistfilePath();
            if (!File.Exists(path))
            {
                return;
            }
            Process.Start("explorer.exe", $"/select,{path}");
        }

        private void OnOpen()
        {
            var path = this.GetPlaylistfilePath();
            if (!File.Exists(path))
            {
                return;
            }
            Process.Start(path);
        }
    }

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
