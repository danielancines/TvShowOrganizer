using Labs.WPF.Core.Notifiers;
using Labs.WPF.TvShowOrganizer.Data.DTO.DTOBase;
using Labs.WPF.TvShowOrganizer.Data.Model;
using System;

namespace Labs.WPF.TvShowOrganizer.Data.DTO
{
    public class EpisodeDTO : DTOObject
    {
        public EpisodeDTO()
        {

        }

        public EpisodeDTO(Episode episode)
        {
            this.ID = episode.ID;
            this.TvShowId = episode.TvShow == null ? Guid.Empty : episode.TvShow.ID;
            this.TvShow = episode.TvShow;
            this.EpisodeId = episode.EpisodeId;
            this.Name = episode.Name;
            this.Number = episode.Number;
            this.Season = episode.Season;
            this.Overview = episode.Overview;
            this.LastUpdated = episode.LastUpdated;
            this._downloaded = episode.Downloaded;
        }

        public Guid ID { get; set; }
        public Guid TvShowId { get; set; }
        public TvShow TvShow { get; set; }
        public int EpisodeId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int Season { get; set; }
        public string Overview { get; set; }
        public double LastUpdated { get; set; }
        public INotify<EpisodeDTO> Notifier { get; set; }

        private bool _downloaded;
        public bool Downloaded
        {
            get { return this._downloaded; }
            set
            {
                if (this._downloaded == value)
                    return;

                this._downloaded = value;

                if (this.Notifier != null && this._downloaded)
                    this.Notifier.Notify(this);
                
                this.RaisePropertyChanged();
            }
        }

        public void SetDownloadedPropertyNotNotify(bool downloaded)
        {
            this._downloaded = downloaded;
            this.RaisePropertyChanged("Downloaded");
        }
    }
}
