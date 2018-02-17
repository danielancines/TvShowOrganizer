using Labs.WPF.TorrentDownload.ViewModels;
using System.Windows;
using Unity.Attributes;

namespace Labs.WPF.TorrentDownload.Views
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        [Dependency]
        public SearchWindowViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
