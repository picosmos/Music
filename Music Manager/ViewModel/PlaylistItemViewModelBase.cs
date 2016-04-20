using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
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
}