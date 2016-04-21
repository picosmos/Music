using System;
using GalaSoft.MvvmLight;

namespace Koopakiller.Apps.MusicManager.ViewModel
{
    public abstract class ReportItemViewModelBase<T> : ViewModelBase where T : ReportItemViewModelBase<T>
    {
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return this._errorMessage; }
            protected set
            {
                this._errorMessage = value;
                this.RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                this.RaisePropertyChanged(nameof(this.HasError));
            }
        }

        public bool HasError => !string.IsNullOrWhiteSpace(this.ErrorMessage);

        public void RemoveItem()
        {
            this.RequestRemoveItem?.Invoke(this, (T)this);
        }

        public event EventHandler<T> RequestRemoveItem;
    }
}