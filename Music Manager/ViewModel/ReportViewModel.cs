using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            this.Items.CollectionChanged += this.OnItemsChanged;

            this.DeleteCommand = new RelayCommand<ReportItemViewModelBase>(this.DeleteItem);
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ReportItemViewModelBase item in e.NewItems)
                {
                    item.RequestRemoveItem += this.OnRequestRemoveItem;
                }
            }
            if (e.OldItems != null)
            {
                foreach (ReportItemViewModelBase item in e.OldItems)
                {
                    item.RequestRemoveItem -= this.OnRequestRemoveItem;
                }
            }
        }

        private void OnRequestRemoveItem(object sender, ReportItemViewModelBase item)
        {
            this.DeleteItem(item);
        }

        public ObservableCollection<ReportItemViewModelBase> Items { get; }

        public ICommand DeleteCommand { get; }

        private void DeleteItem(ReportItemViewModelBase item)
        {
            this.Items.Remove(item);
        }
    }
}
