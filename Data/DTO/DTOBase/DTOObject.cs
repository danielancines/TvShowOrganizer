using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labs.WPF.TvShowOrganizer.Data.DTO.DTOBase
{
    public class DTOObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
