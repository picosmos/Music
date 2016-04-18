using System.Collections.ObjectModel;
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
            this.DeleteCommand = new RelayCommand<ReportItemViewModelBase>(this.DeleteItem);
        }

        public ObservableCollection<ReportItemViewModelBase> Items { get; }

        public ICommand DeleteCommand { get; }

        private void DeleteItem(ReportItemViewModelBase item)
        {
            this.Items.Remove(item);
        }
    }
}
