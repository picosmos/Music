using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

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
}
