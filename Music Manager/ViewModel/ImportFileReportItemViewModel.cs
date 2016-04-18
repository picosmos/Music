using System;
using System.IO;
using System.Security;
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
            try
            {
                var fi = new FileInfo(this.Target);
               
                if (fi.Directory != null && !Directory.Exists(fi.Directory.FullName))
                {
                    Directory.CreateDirectory(fi.Directory.FullName);
                }

                File.Copy(this.Source, this.Target, true);
                this.RemoveItem();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
            {
                this.ErrorMessage = "You have not the required permission to copy the file or access the folder.";
            }
            catch (PathTooLongException)
            {
                this.ErrorMessage = "The specified path or file name is to long.";
            }
            catch (DirectoryNotFoundException)
            {
                this.ErrorMessage = "The specified source or target path is invalid (for example, it is on an unmapped drive).";
            }
            catch (FileNotFoundException)
            {
                this.ErrorMessage = "The source file was not found.";
            }
            catch (IOException)
            {
                this.ErrorMessage = "An unknown I/O error occured.";
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentNullException || ex is ArgumentException)
            {
                this.ErrorMessage = "Invalid file path.";
            }

        }
    }
}