using Labs.WPF.TvShowOrganizer.Data.DTO;
using Prism.Events;

namespace Labs.WPF.TorrentDownload.Events
{
    public class FinishedEditEpisodeEvent : PubSubEvent<EpisodeDTO>
    {
    }
}
