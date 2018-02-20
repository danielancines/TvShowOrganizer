using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.Core.Notifiers;
using Labs.WPF.TorrentDownload.Model;
using Labs.WPF.TorrentDownload.Views;
using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Events;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Resolution;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotify<EpisodeDTO>
    {
        #region Constructor

        public MainWindowViewModel(IUnityContainer container, IEpisodeRepository episodeRepository, ITorrentService torrentService, IEventAggregator eventAggregator, IServerRepository serverRepository, ITvShowDatabase tvShowDatabaseService, ITvShowRepository tvShowRepository)
        {
            this._container = container;
            this._episodeRepository = episodeRepository;
            this._torrentService = torrentService;
            this._eventAggregator = eventAggregator;
            this._serverRepository = serverRepository;
            this._tvShowDatabaseService = tvShowDatabaseService;
            this._tvShowRepository = tvShowRepository;

            this.SearchCommand = new DelegateCommand<object>(this.Execute_Search);
            this.StartDownloadCommand = new DelegateCommand<EpisodeDTO>(this.Execute_StartDownloadCommand);
            this.LoadedCommand = new DelegateCommand<Episode>(this.Execute_LoadedCommand);
            this.ExitCommand = new DelegateCommand<object>(this.Execute_ExitCommand);
            this.Episodes = new ObservableCollection<EpisodeDTO>();
        }

        #endregion

        #region Commands

        public DelegateCommand<object> SearchCommand { get; private set; }
        public DelegateCommand<EpisodeDTO> StartDownloadCommand { get; private set; }
        public DelegateCommand<Episode> LoadedCommand { get; private set; }
        public DelegateCommand<object> ExitCommand { get; private set; }

        #endregion

        #region Fields

        private IUnityContainer _container;
        private IEpisodeRepository _episodeRepository;
        private IServerRepository _serverRepository;
        private ITvShowRepository _tvShowRepository;
        private ITorrentService _torrentService;
        private IEventAggregator _eventAggregator;
        private ITvShowDatabase _tvShowDatabaseService;
        private SubscriptionToken _selectedTorrentToken;

        #endregion

        #region Properties

        public ObservableCollection<EpisodeDTO> Episodes { get; private set; }
        public string SearchTerm { get; set; }

        #endregion

        #region Private Methods

        private void Execute_ExitCommand(object obj)
        {
            var window = ViewsHandler.Instance.GetView("MainWindow") as Window;
            if (window != null)
                window.Close();
        }

        private void Execute_LoadedCommand(Episode obj)
        {
            this.LoadEpisodes();
        }

        private void Execute_Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(this.SearchTerm) && obj != null && obj is string)
                this.SearchTerm = obj.ToString();

            var searchWindow = this._container.Resolve<SearchWindow>(new ParameterOverride("searchTerm", this.SearchTerm));
            ViewsHandler.Instance.RegisterView(searchWindow);
            searchWindow.ShowDialog();
            this.LoadEpisodes();
        }

        private void LoadEpisodes()
        {
            this.Episodes.Clear();
            Task.Factory.StartNew(() =>
            {
                this.UpdateShows();
                this.BusyContent = "Loading Episodes...";
                this.IsBusy = true;

                return this._episodeRepository.NotDownloadedEpisodes();
            }).ContinueWith(load =>
            {
                foreach (var episode in load.Result.OrderBy(e => e.Season).ThenBy(e => e.Number))
                {
                    episode.Notifier = this;
                    this.Episodes.Add(episode);
                }

                this.IsBusy = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void UpdateShows()
        {
            this.BusyContent = "Looking for updates...";
            this.IsBusy = true;

            var server = this._serverRepository.GetServer();

            //var series = this._tvShowRepository.SeriesByLastUpdate(lastUpdated);
            //if (!series.Any())
            //    return;
        }

        private async void Execute_StartDownloadCommand(EpisodeDTO episode)
        {
            if (string.IsNullOrWhiteSpace(episode.TorrentURI))
                this.LoadTorrentData(episode);
            else
                this.DownloadTorrent(episode);
        }

        private void DownloadTorrent(EpisodeDTO episode)
        {
            Process.Start(episode.TorrentURI);
        }

        private async void LoadTorrentData(EpisodeDTO episode)
        {
            this.BusyContent = "Loading torrent data...";
            this.IsBusy = true;

            this._selectedTorrentToken = this._eventAggregator.GetEvent<TorrentSelectedEvent>().Subscribe(this.SelectedTorrent);

            List<Torrent> torrents = new List<Torrent>();
            var links = await this._torrentService.GetLinks(episode.TvShow.Name, episode.Season.ToString("00"), episode.Number.ToString("00"));
            foreach (var item in links)
                torrents.Add(new Torrent(episode.ID, item.Item1, item.Item2));

            var foundWindow = this._container.Resolve<FoundLinksView>(new ParameterOverride("torrents", torrents));
            ViewsHandler.Instance.RegisterView(foundWindow);
            foundWindow.ShowDialog();

            this.IsBusy = false;
        }

        private void SelectedTorrent(Torrent torrent)
        {
            this._eventAggregator.GetEvent<TorrentSelectedEvent>().Unsubscribe(this._selectedTorrentToken);

            var episode = this.Episodes.FirstOrDefault(e => e.ID.Equals(torrent.ParentID));
            if (episode == null)
                return;

            episode.TorrentURI = torrent.MagnetLink;
            this._episodeRepository.UpdateTorrentURI(episode.ID, episode.TorrentURI);
        }

        #endregion

        #region INotify Members

        public void Notify(EpisodeDTO episode)
        {
            Task.Factory.StartNew(() =>
            {
                this.BusyContent = "Saving updates...";
                this.IsBusy = true;

                var updatedEpisodes = this.Episodes.Where(e => e.Season <= episode.Season);

                foreach (var item in updatedEpisodes)
                {
                    if (item.Season == episode.Season && item.Number <= episode.Number)
                        item.SetDownloadedPropertyNotNotify(true);
                    else if (item.Season < episode.Season)
                        item.SetDownloadedPropertyNotNotify(true);
                }

                return updatedEpisodes;
            }).ContinueWith(episodes =>
            {
                foreach (var item in episodes.Result)
                    this._episodeRepository.Update(item);

                this.IsBusy = false;
                this.LoadEpisodes();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
