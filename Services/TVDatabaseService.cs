﻿using Labs.WPF.Core.Converters;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class TVDatabaseService
    {
        #region Constructor

        public TVDatabaseService(XValueConverter xValueConverter, IServerRepository serverRepository, ITvShowRepository tvShowRepository)
        {
            this._xValueConverter = xValueConverter;
            this._serverRepository = serverRepository;
            this._tvShowRepository = tvShowRepository;
        }

        #endregion

        #region Fields

        private XValueConverter _xValueConverter;
        private IServerRepository _serverRepository;
        private ITvShowRepository _tvShowRepository;

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
                    FirstAired = this._xValueConverter.GetValue<DateTime?>(episode.Element("FirstAired")),
                    TvShowId = tvShowId
                });
            }

            return episodes;
        }

        public async Task<double> GetServerUpdate()
        {
            WebClient client = new WebClient();
            var data = await client.DownloadStringTaskAsync("http://thetvdb.com/api/Updates.php?type=none");
            if (data == null)
                return 0;

            var itemElement = XDocument.Parse(data).Descendants("Items").FirstOrDefault();
            return this._xValueConverter.GetValue<double>(itemElement);
        }

        public async Task<bool> UpdateShows()
        {
            //WebClient client = new WebClient();
            //var lastUpdateTime = await this.GetServerUpdate();
            //var server = this._serverRepository.GetServer();

            //if (server.LastUpdate >= lastUpdateTime)
            //    return false;

            //var series = this._tvShowRepository.SeriesByLastUpdate(server.LastUpdate);
            //foreach (var serie in series)
            //{

            //}


            //var server = this._serverRepository.GetServer();
            //var time = server.LastUpdate;
            //var result = false;

            //if (lastUpdateTime <= server.LastUpdate)
            //    return result;

            //do
            //{
            //    server.LastUpdate = time;

            //    var data = await client.DownloadStringTaskAsync(string.Format("http://thetvdb.com/api/Updates.php?type=all&time={0}", server.LastUpdate));
            //    if (data == null)
            //        break;

            //    var timeElement = XDocument.Parse(data).Descendants("Time").FirstOrDefault();
            //    if (timeElement == null)
            //        break;

            //    time = Convert.ToDouble(timeElement.Value);
            //    foreach (var element in XDocument.Parse(data).Descendants("Series"))
            //    {

            //    }

            //} while (server.LastUpdate != lastUpdateTime);

            //this._serverRepository.Update(server);

            return false;
        }

        #endregion
    }
}
