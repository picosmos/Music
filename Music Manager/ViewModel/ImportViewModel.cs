using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Koopakiller.Apps.MusicManager.Dialogs;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ImportViewModel : HeaderViewModelBase
    {
        public ImportViewModel() : base("Import")
        {
            this.OpenCommand = new RelayCommand(this.OnOpen);
            this.DragOverCommand = new RelayCommand<DragEventArgs>(this.OnDragOver);
            this.DropCommand = new RelayCommand<DragEventArgs>(this.OnDrop);
        }

        private readonly string[] _supportedFileExtensions = new[] { ".mp3", ".wav", ".m4a", ".flac" };

        private void OnDragOver(DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Contains(DataFormats.FileDrop))
            {
                var fis = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Select(x => new FileInfo(x))
                    .Where(x => this._supportedFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

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
                    .Where(x => this._supportedFileExtensions.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

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
            ReportViewModel.Instance.Items.Add(new ImportFileReportItemViewModel(fi));
        }
    }
}
