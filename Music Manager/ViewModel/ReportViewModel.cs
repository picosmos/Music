using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ReportViewModel : HeaderViewModelBase
    {
        public static ReportViewModel Instance { get; } = new ReportViewModel();

        private ReportViewModel() : base("Report")
        {
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
    }
}
