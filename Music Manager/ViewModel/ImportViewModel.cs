using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Koopakiller.Apps.MusicManager.Dialogs;
using Koopakiller.Apps.MusicManager.Helper;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ImportViewModel : HeaderViewModelBase
    {
        public ImportViewModel() : base("Import")
        {
            this.OpenCommand = new RelayCommand(this.OnOpen);
            this.DragOverCommand = new RelayCommand<DragEventArgs>(this.OnDragOver);
            this.DropCommand = new RelayCommand<DragEventArgs>(this.OnDrop);

            this.Items = new ObservableCollection<ReportItemViewModelBase>();
            this.ImportAllCommand = new RelayCommand(this.OnImportAll, () => this.Items.OfType<ImportFileReportItemViewModel>().Any(x => x.ImportShouldBePossible));
            this.Items.CollectionChanged += this.OnItemsChanged;

            this.DeleteCommand = new RelayCommand<ReportItemViewModelBase>(this.DeleteItem);
        }
        private void OnImportAll()
        {
            foreach (var file in this.Items.OfType<ImportFileReportItemViewModel>()
                                           .Where(x => x.ImportShouldBePossible)
                                           .ToArray())
            {
                file.Import();
            }
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ReportItemViewModelBase item in e.NewItems)
                {
                    item.RequestRemoveItem += this.OnRequestRemoveItem;
                    item.PropertyChanged += this.OnItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ReportItemViewModelBase item in e.OldItems)
                {
                    item.RequestRemoveItem -= this.OnRequestRemoveItem;
                    item.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }

            ((RelayCommand)this.ImportAllCommand).RaiseCanExecuteChanged();
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ImportFileReportItemViewModel.ImportShouldBePossible):
                    ((RelayCommand)this.ImportAllCommand).RaiseCanExecuteChanged();
                    break;
            }
        }

        private void OnRequestRemoveItem(object sender, ReportItemViewModelBase item)
        {
            this.DeleteItem(item);
        }

        public ObservableCollection<ReportItemViewModelBase> Items { get; }

        public ICommand DeleteCommand { get; }
        public ICommand ImportAllCommand { get; }

        private void DeleteItem(ReportItemViewModelBase item)
        {
            this.Items.Remove(item);
        }

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
                var fis = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new FileInfo(x))
                    .Where(x => FileSystemHelper.SupportedMusicFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

                foreach (var fi in fis)
                {
                    this.ProcessFile(fi);
                }
            }
        }

        public RelayCommand<DragEventArgs> DragOverCommand { get; }
        public RelayCommand<DragEventArgs> DropCommand { get; }
        public ICommand OpenCommand { get; }

        private void OnOpen()
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
                this.ProcessFile(new FileInfo(file));
            }
        }

        private void ProcessFile(FileInfo fi)
        {
            this.Items.Add(new ImportFileReportItemViewModel(fi));
        }
    }
}
