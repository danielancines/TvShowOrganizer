using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.TorrentDownload.Events;
using Labs.WPF.TvShowOrganizer.Data.DTO;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class EditEpisodeViewModel : ViewModelBase
    {
        #region Constructor

        public EditEpisodeViewModel(EpisodeDTO episode, Guid windowId, IEventAggregator eventAggregator)
        {
            this.Episode = episode;
            this._windowId = windowId;
            this._eventAggregator = eventAggregator;

            this.WindowTitle = string.Format("Editing Show: {0} - {1}", this.Episode.TvShow.Name, this.Episode.Name);
            this.OkCommand = new DelegateCommand<object>(this.Execute_OkCommand);
            this.CancelCommand = new DelegateCommand<object>(this.Execute_CancelCommand);
            this.DownloadedOptions = new Dictionary<string, bool>();
            this.DownloadedOptions.Add("Yes", true);
            this.DownloadedOptions.Add("No", false);
        }

        #endregion

        #region Fields

        private Guid _windowId;
        private IEventAggregator _eventAggregator;

        #endregion

        #region Commands

        public DelegateCommand<object> OkCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }

        #endregion

        #region Properties

        public EpisodeDTO Episode { get; private set; }
        public string WindowTitle { get; private set; }
        public Dictionary<string, bool> DownloadedOptions { get; private set; }

        #endregion

        #region Private Methods

        private void Execute_CancelCommand(object obj)
        {
            var window = ViewsHandler.Instance.GetView(this._windowId) as Window;
            if (window != null)
                window.Close();
        }

        private void Execute_OkCommand(object obj)
        {
            var window = ViewsHandler.Instance.GetView(this._windowId) as Window;
            if (window == null)
                return;

            this.Episode.TorrentURI = string.IsNullOrWhiteSpace(this.Episode.TorrentURI) ? null : this.Episode.TorrentURI;
            this._eventAggregator.GetEvent<FinishedEditEpisodeEvent>().Publish(this.Episode);
            window.Close();
        }

        #endregion
    }
}
