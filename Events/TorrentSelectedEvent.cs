using Labs.WPF.TorrentDownload.Model;
using Prism.Events;

namespace Labs.WPF.TvShowOrganizer.Events
{
    public class TorrentSelectedEvent : PubSubEvent<Torrent>
    {
    }
}
