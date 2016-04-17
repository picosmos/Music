using GalaSoft.MvvmLight;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public class HeaderViewModelBase : ViewModelBase
    {
        public HeaderViewModelBase(string header)
        {
            this.Header = header;
        }

        private string _header;

        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                this.RaisePropertyChanged();
            }
        }
    }
}