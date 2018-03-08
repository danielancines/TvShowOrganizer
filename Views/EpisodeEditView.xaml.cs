using Labs.WPF.TorrentDownload.ViewModels;
using System.Windows;
using Unity.Attributes;

namespace Labs.WPF.TorrentDownload.Views
{
    /// <summary>
    /// Interaction logic for EpisodeEditView.xaml
    /// </summary>
    public partial class EpisodeEditView : Window
    {
        public EpisodeEditView()
        {
            InitializeComponent();
        }

        [Dependency]
        public EditEpisodeViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
