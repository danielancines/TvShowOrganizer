﻿using Labs.WPF.Core;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Resolution;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotify<EpisodeDTO>
    {
        #region Constructor

        public MainWindowViewModel(IUnityContainer container, IEpisodeRepository episodeRepository, ITorrentService torrentService, IEventAggregator eventAggregator, IServerRepository serverRepository, ITvShowDatabase tvShowDatabaseService, ITvShowRepository tvShowRepository, IMessageService messageService, IInternetService internetService)
        {
            this._container = container;
            this._episodeRepository = episodeRepository;
            this._torrentService = torrentService;
            this._eventAggregator = eventAggregator;
            this._serverRepository = serverRepository;
            this._tvShowDatabaseService = tvShowDatabaseService;
            this._tvShowRepository = tvShowRepository;
            this._messageService = messageService;
            this._internetService = internetService;

            this.SearchCommand = new DelegateCommand<object>(this.Execute_Search);
            this.StartDownloadCommand = new DelegateCommand<object>(this.Execute_StartDownloadCommand);
            this.LoadedCommand = new DelegateCommand<Episode>(this.Execute_LoadedCommand);
            this.ExitCommand = new DelegateCommand<object>(this.Execute_ExitCommand);
            this.UpdateCommand = new DelegateCommand<object>(this.Execute_UpdateCommand);
            this.MarkEpisodeAsDownloadedCommand = new DelegateCommand<object>(this.Execute_MarkEpisodeAsDownloadedCommand);
            this.Episodes = new ObservableCollection<EpisodeDTO>();
        }

        private void Execute_MarkEpisodeAsDownloadedCommand(object obj)
        {
            this.MarkEpisodeAsDownloaded(this.SelectedEpisode);
        }

        #endregion

        #region Commands

        public DelegateCommand<object> SearchCommand { get; private set; }
        public DelegateCommand<object> StartDownloadCommand { get; private set; }
        public DelegateCommand<Episode> LoadedCommand { get; private set; }
        public DelegateCommand<object> ExitCommand { get; private set; }
        public DelegateCommand<object> UpdateCommand { get; private set; }
        public DelegateCommand<object> MarkEpisodeAsDownloadedCommand { get; private set; }

        #endregion

        #region Fields

        private IUnityContainer _container;
        private IEpisodeRepository _episodeRepository;
        private IServerRepository _serverRepository;
        private ITvShowRepository _tvShowRepository;
        private ITorrentService _torrentService;
        private IEventAggregator _eventAggregator;
        private ITvShowDatabase _tvShowDatabaseService;
        private IMessageService _messageService;
        private IInternetService _internetService;
        private SubscriptionToken _selectedTorrentToken;

        #endregion

        #region Properties

        public ObservableCollection<EpisodeDTO> Episodes { get; private set; }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return this._searchTerm; }
            set
            {
                if (this._searchTerm == value)
                    return;

                this._searchTerm = value;
                this.RaisePropertyChanged();
            }
        }

        private EpisodeDTO _selectedEpisode;
        public EpisodeDTO SelectedEpisode
        {
            get { return this._selectedEpisode; }
            set
            {
                if (this._selectedEpisode == value)
                    return;

                this._selectedEpisode = value;

                if (this.SelectedEpisode != null)
                    this.CanDownload = !string.IsNullOrWhiteSpace(this._selectedEpisode.TorrentURI);
                this.RaisePropertyChanged();
            }
        }

        private bool _showingDonwloaded = false;
        public bool ShowingDonwloaded
        {
            get { return this._showingDonwloaded; }
            set
            {
                if (this._showingDonwloaded == value)
                    return;

                this._showingDonwloaded = value;
                this.LoadDownloadedEpisodes(value);
                this.RaisePropertyChanged();
            }
        }

        private bool _canDownload = false;
        public bool CanDownload
        {
            get { return this._canDownload; }
            set
            {
                if (this._canDownload == value)
                    return;

                this._canDownload = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Private Methods

        private void ChangeShowingDownloadedNotNotify(bool value)
        {
            this._showingDonwloaded = value;
            this.RaisePropertyChanged("ShowingDonwloaded");
        }

        private void LoadDownloadedEpisodes(bool show)
        {
            this.Episodes.Clear();
            this.IsBusy = true;
            this.BusyContent = "Updating list...";
            Task.Factory.StartNew(() =>
            {
                if (show)
                    return this._episodeRepository.DownloadedEpisodes().OrderBy(e => e.Season).ThenBy(e => e.Number);
                else
                    return this._episodeRepository.NotDownloadedEpisodes().OrderBy(e => e.Season).ThenBy(e => e.Number);
            }).ContinueWith(episodes =>
            {
                this.Episodes.AddRange(episodes.Result);
                this.IsBusy = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Execute_SearchTorrentLinksCommand(object obj)
        {
            if (this.SelectedEpisode == null)
                return;

            this.LoadTorrentData(this.SelectedEpisode);
        }

        private void Execute_UpdateCommand(object obj)
        {
            this.LoadEpisodes();
        }

        private void Execute_ExitCommand(object obj)
        {
            var window = ViewsHandler.Instance.GetView("MainWindow") as Window;
            if (window != null)
                window.Close();
        }

        private void Execute_LoadedCommand(Episode obj)
        {
            this.LoadEpisodes();
            this.StartInternetConnectionVerifier();
        }

        private void StartInternetConnectionVerifier()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!this._internetService.HasInternetConnection())
                        this.ErrorMessage = "No internet connection available!";

                    Thread.Sleep(30000);
                    this.ErrorMessage = string.Empty;
                }
            });
        }

        private void Execute_Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(this.SearchTerm) && obj != null && obj is string)
                this.SearchTerm = obj.ToString();

            var searchWindow = this._container.Resolve<SearchWindow>(new ParameterOverride("searchTerm", this.SearchTerm));
            ViewsHandler.Instance.RegisterView(searchWindow);
            searchWindow.ShowDialog();
            this.LoadEpisodes();
            this.SearchTerm = string.Empty;
        }

        private void LoadEpisodes()
        {
            this.ChangeShowingDownloadedNotNotify(false);
            this.Episodes.Clear();
            Task.Factory.StartNew(() =>
            {
                this.BusyContent = "Looking for updates...";
                this.IsBusy = true;
                this._tvShowDatabaseService.UpdateShows().Wait();
                this.BusyContent = "Loading episodes...";

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

        private async Task<bool> UpdateShows()
        {
            return await this._tvShowDatabaseService.UpdateShows();
        }

        private async void Execute_StartDownloadCommand(object obj)
        {
            if (this.SelectedEpisode == null)
                return;

            if (string.IsNullOrWhiteSpace(this.SelectedEpisode.TorrentURI))
                this.LoadTorrentData(this.SelectedEpisode);
            else
                this.DownloadTorrent(this.SelectedEpisode);
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
            //this.MarkEpisodeAsDownloaded(episode);
        }

        private void MarkEpisodeAsDownloaded(EpisodeDTO episode)
        {
            var messageResult = this._messageService.Show("Set downloaded to previous episodes?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (messageResult == MessageBoxResult.Cancel)
                return;

            if (messageResult == MessageBoxResult.No)
            {
                this._episodeRepository.Update(episode);
                this.Episodes.Remove(episode);
                return;
            }

            Task.Factory.StartNew(() =>
            {
                this.BusyContent = "Saving updates...";
                this.IsBusy = true;

                var updatedEpisodes = new List<EpisodeDTO>();

                foreach (var item in this.Episodes.Where(e => e.Season <= episode.Season))
                {
                    if (item.Season == episode.Season && item.Number <= episode.Number)
                    {
                        item.SetDownloadedPropertyNotNotify(true);
                        updatedEpisodes.Add(item);
                    }
                    else if (item.Season < episode.Season)
                    {
                        item.SetDownloadedPropertyNotNotify(true);
                        updatedEpisodes.Add(item);
                    }
                }

                return updatedEpisodes;
            }).ContinueWith(episodes =>
            {
                foreach (var item in episodes.Result)
                {
                    if (this._episodeRepository.Update(item))
                        this.Episodes.Remove(item);
                }

                this.IsBusy = false;
                //this.LoadEpisodes();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
