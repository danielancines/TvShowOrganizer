using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.Core.Notifiers;
using Labs.WPF.TorrentDownload.Views;
using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Resolution;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotify<EpisodeDTO>
    {
        #region Constructor

        public MainWindowViewModel(IUnityContainer container, IEpisodeRepository episodeRepository)
        {
            this._container = container;
            this._episodeRepository = episodeRepository;
            this.SearchCommand = new DelegateCommand<object>(this.Execute_Search);
            this.StartDownloadCommand = new DelegateCommand<Episode>(this.Execute_StartDownloadCommand);
            this.LoadedCommand = new DelegateCommand<Episode>(this.Execute_LoadedCommand);
            this.Episodes = new ObservableCollection<EpisodeDTO>();
        }

        #endregion

        #region Commands

        public DelegateCommand<object> SearchCommand { get; private set; }
        public DelegateCommand<Episode> StartDownloadCommand { get; private set; }
        public DelegateCommand<Episode> LoadedCommand { get; private set; }

        #endregion

        #region Fields

        private IUnityContainer _container;
        private IEpisodeRepository _episodeRepository;

        #endregion

        #region Properties

        public ObservableCollection<EpisodeDTO> Episodes { get; private set; }
        public string SearchTerm { get; set; }

        #endregion

        #region Private Methods

        private void Execute_LoadedCommand(Episode obj)
        {
            this.LoadEpisodes();
        }

        private void Execute_Search(object obj)
        {
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
            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Execute_StartDownloadCommand(Episode episode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotify Members

        public void Notify(EpisodeDTO episode)
        {
            Task.Factory.StartNew(() =>
            {
                this.BusyContent = "Saving updates...";
                this.IsBusy = true;

                var updatedEpisodes = this.Episodes.Where(e => e.Season <= episode.Season && e.Number <= episode.Number);

                foreach (var item in updatedEpisodes)
                    item.SetDownloadedPropertyNotNotify(true);

                return updatedEpisodes;
            }).ContinueWith(episodes =>
            {
                foreach (var item in episodes.Result)
                    this._episodeRepository.Update(item);

                this.LoadEpisodes();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
