using Labs.WPF.Core.Handlers;
using Labs.WPF.TvShowOrganizer.Data;
using Labs.WPF.TvShowOrganizer.Data.Repositories;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Services;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using Prism.Events;
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
            container.RegisterType<ITvShowDatabase, TheMovieDbService>();
            container.RegisterType<DbContext, TvShowOrganizerContext>();
            container.RegisterType<ITvShowRepository, TvShowRepository>();
            container.RegisterType<IEpisodeRepository, EpisodeRepository>();
            container.RegisterType<IServerRepository, TvShowServerRepository>();
            container.RegisterType<ITorrentService, TorrentService>();
            container.RegisterType<IMessageService, MessageBoxService>();
            container.RegisterInstance<IEventAggregator>(new EventAggregator());

            var mainWindow = container.Resolve<MainWindow>();
            ViewsHandler.Instance.RegisterView(mainWindow);

            mainWindow.Show();
        }
    }
}
