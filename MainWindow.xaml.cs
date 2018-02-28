using Labs.WPF.TorrentDownload.ViewModels;
using System;
using System.Reflection;
using System.Windows;
using Unity.Attributes;

namespace Labs.WPF.TorrentDownload
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        [Dependency]
        public MainWindowViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            this.Title = string.Format("TV Shows Organizer - V.{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}
