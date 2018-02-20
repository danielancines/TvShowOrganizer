using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class SearchWindowViewModel : ViewModelBase
    {
        #region Constructor

        public SearchWindowViewModel(ITvShowDatabase tvDatabaseService, string searchTerm, ITvShowRepository tvShowRepository, IEpisodeRepository episodeRepository)
        {
            this._tvDatabaseService = tvDatabaseService;
            this._searchTerm = searchTerm;
            this._tvShowRepository = tvShowRepository;
            this._episodeRepository = episodeRepository;

            this.OKCommand = new DelegateCommand<object>(this.Execute_OKCommand);
            //this.CancelCommand = new DelegateCommand<object>(this.Execute_CancelCommand);
            this.LoadedCommand = new DelegateCommand<object>(this.Execute_LoadedCommand);
            this.SelectItemCommand = new DelegateCommand<TvShow>(this.Execute_SelectItemCommand);
            this.Shows = new ObservableCollection<TvShow>();
        }

        #endregion

        #region Fields

        private ITvShowDatabase _tvDatabaseService;
        private string _searchTerm;
        private ITvShowRepository _tvShowRepository;
        private IEpisodeRepository _episodeRepository;
        

        #endregion

        #region Commands

        public DelegateCommand<object> OKCommand { get; set; }
        //public DelegateCommand<object> CancelCommand { get; set; }
        public DelegateCommand<object> LoadedCommand { get; set; }
        public DelegateCommand<TvShow> SelectItemCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<TvShow> Shows { get; private set; }

        #endregion

        #region Private Methods

        private async void Execute_SelectItemCommand(TvShow tvShow)
        {
            if (this._tvShowRepository.Exists(tvShow.SeriesID))
            {
                this.ErrorMessage = "Selected Show is already in your list!";
                return;
            }

            this.IsBusy = true;
            this.BusyContent = string.Format("Saving {0}", tvShow.Name);
            tvShow.LastUpdated = await this._tvDatabaseService.GetServerUpdate();

            this._tvShowRepository.Add(tvShow);
            this.BusyContent = string.Format("Loading episodes...", tvShow.Name);

            var episodes = await this._tvDatabaseService.GetEpisodes(tvShow.SeriesID, tvShow.ID);
            this.BusyContent = string.Format("{0} episodes founded. Saving...", episodes.Count());

            this._episodeRepository.AddRange(episodes);

            this.IsBusy = false;

            this.CloseWindow();
        }

        private async void Execute_LoadedCommand(object obj)
        {
            this.IsBusy = true;
            this.BusyContent = string.Format("Searching: {0}", this._searchTerm);

            try
            {
                var shows = await this._tvDatabaseService.Search(this._searchTerm);
                this.BusyContent = string.Format("{0} shows founded, loading list...", shows.Count());

                foreach (var show in shows)
                    this.Shows.Add(show);
            }
            catch (WebException webException)
            {

            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void Execute_OKCommand(object obj)
        {
            this.CloseWindow();
        }

        //private void Execute_CancelCommand(object obj)
        //{
        //    this.CloseWindow();
        //}

        private void CloseWindow()
        {
            var view = ViewsHandler.Instance.GetView("SearchWindow") as Window;
            if (view == null)
                return;

            view.Close();
        }

        #endregion
    }
}
