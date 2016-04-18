using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ImportFileReportItemViewModel : ReportItemViewModelBase
    {
        public ImportFileReportItemViewModel(FileInfo fi)
        {
            this._fi = fi;
            this.Source = this._fi.FullName;
            this.Target = this.BuildTargetPath();
            this.RaiseTargetIsValidChanged();

            this.ImportCommand = new RelayCommand(this.OnImport);
        }

        private readonly FileInfo _fi;
        private string _target;
        private bool _targetFileAlreadyExists;

        private string BuildTargetPath()
        {
            var tag = TagLib.File.Create(this._fi.FullName).Tag;
            var pb = new PathBuilder(Properties.Settings.Default.MusicPath, Properties.Settings.Default.ImportPathPattern)
            {
                Replacements =
                {
                    ["Extension"] = this._fi.Extension,
                    ["Title"] = tag.Title,
                    ["JoinedAlbumArtists"] = tag.JoinedAlbumArtists,
                    ["JoinedPerformers"] = tag.JoinedPerformers,
                    ["Album"] = tag.Album,
                    ["JoinedGenres"] = tag.JoinedGenres,
                }
            };

            return Path.GetFullPath(pb.Build());
        }

        public string Source { get; }

        public string Target
        {
            get { return this._target; }
            set
            {
                this._target = value;
                this.TargetFileAlreadyExists = File.Exists(this.Target);
                this.RaisePropertyChanged();
                this.RaiseTargetIsValidChanged();
            }
        }

        public bool TargetFileAlreadyExists
        {
            get { return this._targetFileAlreadyExists; }
            private set
            {
                this._targetFileAlreadyExists = value;
                this.RaisePropertyChanged();
            }
        }

        public bool TargetIsValid
        {
            get
            {
                try
                {
                    //checks for illegal characters too
                    return Path.GetFullPath(this.Source) != Path.GetFullPath(this.Target);
                }
                catch
                {
                    return false;
                }
            }
        }

        protected void RaiseTargetIsValidChanged()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged(nameof(this.TargetIsValid));
        }

        public ICommand ImportCommand { get; }

        private void OnImport()
        {
            File.Copy(this.Source, this.Target);
        }
    }
}