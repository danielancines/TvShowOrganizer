using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.TorrentDownload.Model;
using Labs.WPF.TvShowOrganizer.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class FoundLinksViewModel : ViewModelBase
    {
        #region Constructor

        public FoundLinksViewModel(IEnumerable<Torrent> torrents, Guid windowId, IEventAggregator eventAggregator)
        {
            this.Torrents = new ObservableCollection<Torrent>(torrents);
            this._windowId = windowId;
            this._eventAggregator = eventAggregator;

            this.SelectItemCommand = new DelegateCommand<Torrent>(this.Execute_SelecItemCommand);
        }

        #endregion

        #region Fields

        private IEventAggregator _eventAggregator;
        private Guid _windowId;

        #endregion

        #region Properties

        public DelegateCommand<Torrent> SelectItemCommand { get; private set; }
        public ObservableCollection<Torrent> Torrents { get; private set; }

        #endregion

        #region Private Methods

        private void Execute_SelecItemCommand(Torrent selectedTorrent)
        {
            var view = ViewsHandler.Instance.GetView(this._windowId) as Window;
            if (view == null)
                return;

            this._eventAggregator.GetEvent<TorrentSelectedEvent>().Publish(selectedTorrent);
            view.Close();
        }

        #endregion
    }
}
