using Labs.WPF.TvShowOrganizer.Data;
using Labs.WPF.TvShowOrganizer.Data.Repositories;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Services;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System.Data.Entity;
using System.Windows;
using Unity;

namespace Labs.WPF.TorrentDownload
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();
            container.RegisterType<ITvShowDatabase, TVDatabaseService>();
            container.RegisterType<DbContext, TvShowOrganizerContext>();
            container.RegisterType<ITvShowRepository, TvShowRepository>();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
