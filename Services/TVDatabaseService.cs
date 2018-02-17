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

        public TVDatabaseService()
        {

        }

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
                    Banner = tvShow.Element("banner") == null ? string.Empty : tvShow.Element("banner") == null ? string.Empty : "http://thetvdb.com/banners/" + tvShow.Element("banner").Value,
                    Name = tvShow.Element("SeriesName") == null ? string.Empty : tvShow.Element("SeriesName").Value,
                    SeriesID = tvShow.Element("seriesid") == null ? 0 : Convert.ToInt32(tvShow.Element("seriesid").Value),
                    Language = tvShow.Element("language") == null ? string.Empty : tvShow.Element("language").Value,
                    Overview = tvShow.Element("Overview") == null ? string.Empty : tvShow.Element("Overview").Value,
                    FirstAired = tvShow.Element("FirstAired") == null ? new DateTime?() : Convert.ToDateTime(tvShow.Element("FirstAired").Value),
                    Network = tvShow.Element("Network") == null ? string.Empty : tvShow.Element("Network").Value,
                    ImdbId = tvShow.Element("IMDB_ID") == null ? string.Empty : tvShow.Element("IMDB_ID").Value,
                    DatabaseId = tvShow.Element("id") == null ? string.Empty : tvShow.Element("id").Value
                };

                shows.Add(newTvShow);
            }

            return shows;
        }

        #endregion
    }
}
