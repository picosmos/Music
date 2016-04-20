using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Koopakiller.Apps.MusicManager.Dialogs;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class PlaylistItemViewModel : PlaylistItemViewModelBase
    {
        private bool _hasUnsavedChanges;

        public PlaylistItemViewModel()
        {
            this.SaveCommand = new RelayCommand(this.Save, () => this.HasUnsavedChanges);
            this.AddMusicFileCommand = new RelayCommand(this.OnAddMusicFile);
            this.DragOverCommand = new RelayCommand<DragEventArgs>(this.OnDragOver);
            this.DropCommand = new RelayCommand<DragEventArgs>(this.OnDrop);

            var oc = new ObservableCollection<string>();
            oc.CollectionChanged += this.OnSongFilesChanged;
            this.SongFiles = oc;

        }

        protected override void RaisePropertyChanged(string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            base.RaisePropertyChanged(propertyName);

            if (propertyName == nameof(this.Name))
            {
                this.HasUnsavedChanges = true;
            }
        }

        public bool HasUnsavedChanges
        {
            get { return this._hasUnsavedChanges; }
            set
            {
                this._hasUnsavedChanges = value;
                this.RaisePropertyChanged();
                ((RelayCommand)this.SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void OnSongFilesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (string file in e.NewItems)
                {
                    var fi = new FileInfo(file);
                    if (!FileSystemHelper.SupportedMusicFileExtensions.Any(x => fi.Extension.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        throw new ArgumentException($"Invalid file type by file '{file}'");
                    }
                }
            }
            this.HasUnsavedChanges = true;
        }

        public IList<string> SongFiles { get; }

        public ICommand AddMusicFileCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand SaveCommand { get; }


        private void OnDragOver(DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Contains(DataFormats.FileDrop))
            {
                var fis = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new FileInfo(x))
                    .Where(x => FileSystemHelper.SupportedMusicFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (fis.Any())
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDrop(DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Contains(DataFormats.FileDrop))
            {
                foreach (var file in ((string[])e.Data.GetData(DataFormats.FileDrop)))
                {
                    var fi = new FileInfo(file);
                    if (FileSystemHelper.SupportedMusicFileExtensions.Any(y => y.Equals(fi.Extension, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        this.SongFiles.Add(file);
                    }
                }
            }
        }


        private void OnAddMusicFile()
        {
            if (DialogFactories.OpenFileDialogFactory == null)
            {
                throw new NullReferenceException($"{nameof(DialogFactories.OpenFileDialogFactory)} is not assigned");
            }

            var dlg = DialogFactories.OpenFileDialogFactory.CreateDialog();
            dlg.Filter = "Music files (*.flac;*.mp3;*.wav;*.m4a)|*.flac;*.mp3;*.wav;*.m4a";
            dlg.Multiselect = true;
            if (!dlg.Show())
            {
                return;
            }

            foreach (var file in dlg.FileNames)
            {
                this.SongFiles.Add(file);
            }
        }


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
                foreach (var file in this.SongFiles)
                {
                    sw.WriteLine(file);
                }
            }
            this.HasUnsavedChanges = false;
        }

        public static PlaylistItemViewModel LoadFile(string file)
        {
            var pl = new PlaylistItemViewModel
            {
                Name = Path.GetFileNameWithoutExtension(file)
            };
            foreach (var line in File.ReadLines(file).Where(x => !x.StartsWith("#")))
            {
                pl.SongFiles.Add(line);
            }
            return pl;
        }

    }
}