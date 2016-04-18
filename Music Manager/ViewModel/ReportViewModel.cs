using System.Collections.ObjectModel;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class ReportViewModel : HeaderViewModelBase
    {
        public static ReportViewModel Instance { get; } = new ReportViewModel();

        private ReportViewModel() : base("Report")
        {
            this.Items = new ObservableCollection<ReportItemViewModelBase>();
        }

        public ObservableCollection<ReportItemViewModelBase> Items { get; }
    }
}
