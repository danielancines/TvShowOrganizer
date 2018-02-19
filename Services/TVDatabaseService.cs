using Labs.WPF.Core.Converters;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class TVDatabaseService : ITvShowDatabase
    {
        #region Constructor

        public TVDatabaseService(XValueConverter xValueConverter)
        {
            this._xValueConverter = xValueConverter;
        }

        #endregion

        #region Fields

        private XValueConverter _xValueConverter;

        #endregion

        #region ITvShowDatabase Members

        public async Task<IEnumerable<TvShow>> Search(string term)
        {
            var shows = new List<TvShow>();
            WebClient client = new WebClient();
            var data = await client.DownloadStringTaskAsync(string.Format("http://thetvdb.com/api/GetSeries.php?seriesname={0}", term));
            if (data == null)
                return null;

            foreach (var tvShow in XDocument.Parse(data).Descendants("Series"))
            {
                var newTvShow = new TvShow()
                {
                    ID = Guid.NewGuid(),
                    Banner = "http://thetvdb.com/banners/" + this._xValueConverter.GetValue<string>(tvShow.Element("banner")),
                    Name = this._xValueConverter.GetValue<string>(tvShow.Element("SeriesName")),
                    SeriesID = this._xValueConverter.GetValue<int>(tvShow.Element("seriesid")),
                    Language = this._xValueConverter.GetValue<string>(tvShow.Element("language")),
                    Overview = this._xValueConverter.GetValue<string>(tvShow.Element("Overview")),
                    FirstAired = this._xValueConverter.GetValue<DateTime>(tvShow.Element("FirstAired")),
                    Network = this._xValueConverter.GetValue<string>(tvShow.Element("Network")),
                    ImdbId = this._xValueConverter.GetValue<string>(tvShow.Element("IMDB_ID")),
                    DatabaseId = this._xValueConverter.GetValue<string>(tvShow.Element("id"))
                };

                shows.Add(newTvShow);
            }

            return shows;
        }

        public async Task<IEnumerable<Episode>> GetEpisodes(int serieID, Guid tvShowId)
        {
            var episodes = new List<Episode>();
            WebClient client = new WebClient();
            var data = await client.DownloadStringTaskAsync(string.Format("http://thetvdb.com/api/51CE3E7B0101F341/series/{0}/all/en.xml", serieID));
            if (data == null)
                return null;

            foreach (var episode in XDocument.Parse(data).Descendants("Episode"))
            {
                episodes.Add(new Episode()
                {
                    ID = Guid.NewGuid(),
                    Name = this._xValueConverter.GetValue<string>(episode.Element("EpisodeName")),
                    EpisodeId = this._xValueConverter.GetValue<int>(episode.Element("id")),
                    Number = this._xValueConverter.GetValue<int>(episode.Element("EpisodeNumber")),
                    LastUpdated = this._xValueConverter.GetValue<double>(episode.Element("lastupdated")),
                    Overview = this._xValueConverter.GetValue<string>(episode.Element("Overview")),
                    Season = this._xValueConverter.GetValue<int>(episode.Element("SeasonNumber")),
                    TvShowId = tvShowId
                });
            }

            return episodes;
        }

        #endregion
    }
}
