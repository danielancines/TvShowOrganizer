using Labs.WPF.TorrentDownload.ViewModels;
using System.Windows;
using Unity.Attributes;

namespace Labs.WPF.TorrentDownload.Views
{
    /// <summary>
    /// Interaction logic for FoundLinksView.xaml
    /// </summary>
    public partial class FoundLinksView : Window
    {
        public FoundLinksView()
        {
            InitializeComponent();
        }

        [Dependency]
        public FoundLinksViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
