using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;
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

            this.LoadPlaylistFiles();
        }

        void LoadPlaylistFiles()
        {
            var path = Properties.Settings.Default.PlaylistsPath;
            if (Directory.Exists(path))
            {
                foreach (var file in FileSystemHelper.GetFilesFromDirectory(path, new string[] { ".m3u" }))
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        if (line == "#" + AllSongsPlaylistViewModel.FileIdentifier)
                        {
                            this.Playlists.Add(new AllSongsPlaylistViewModel()
                            {
                                Name = Path.GetFileNameWithoutExtension(file),
                            });
                        }
                        else
                        {
                            this.Playlists.Add(PlayListItemViewModel.LoadFile(file));
                        }
                        break;//read only first file
                    }
                }
            }
        }

        private void OnPlaylistsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (PlaylistItemViewModelBase playlist in e.NewItems)
                {
                    playlist.DeleteRequested += this.OnPlayListDeleteRequested;
                }
            }
            if (e.OldItems != null)
            {
                foreach (PlaylistItemViewModelBase playlist in e.OldItems)
                {
                    playlist.DeleteRequested -= this.OnPlayListDeleteRequested;
                }
            }
            ((RelayCommand)this.AddAllSongsPlaylistCommand).RaiseCanExecuteChanged();
        }

        private void OnPlayListDeleteRequested(object sender, PlaylistItemViewModelBase item)
        {
            this.Playlists.Remove(item);
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
}
