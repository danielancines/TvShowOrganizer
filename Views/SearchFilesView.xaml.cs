using Labs.WPF.TorrentDownload.ViewModels;
using System.Windows;
using Unity.Attributes;

namespace Labs.WPF.TorrentDownload.Views
{
    /// <summary>
    /// Interaction logic for SearchFilesView.xaml
    /// </summary>
    public partial class SearchFilesView : Window
    {
        public SearchFilesView()
        {
            InitializeComponent();
        }

        [Dependency]
        public SearchFilesViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
