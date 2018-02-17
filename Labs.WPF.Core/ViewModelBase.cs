using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labs.WPF.Core
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Properties

        private string _busyContent;
        public string BusyContent
        {
            get { return this._busyContent; }
            set
            {
                if (this._busyContent == value)
                    return;

                this._busyContent = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                if (this._isBusy == value)
                    return;

                this._isBusy = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
