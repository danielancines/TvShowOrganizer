using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class TheMovieDbService : ITvShowDatabase
    {
        #region Constructor

        public TheMovieDbService(IServerRepository serverRepository, ITvShowRepository tvShowRepository, IEpisodeRepository episodeRepository)
        {
            this._serverRepository = serverRepository;
            this._tvShowRepository = tvShowRepository;
            this._episodeRepository = episodeRepository;
        }

        #endregion

        #region Fields

        private IServerRepository _serverRepository;
        private ITvShowRepository _tvShowRepository;
        private IEpisodeRepository _episodeRepository;

        #endregion

        #region ITvShowDatabase Members

        public async Task<IEnumerable<Episode>> GetEpisodes(int serieID, Guid tvShowId)
        {
            var server = this._serverRepository.GetServer();
            var episodes = new List<Episode>();
            HttpClient client = new HttpClient() { BaseAddress = new Uri(server.BaseUri) };
            var response = await client.GetStringAsync(string.Format(Uris.SeasonsURI, serieID, server.ApiKey));
            var jObject = JObject.Parse(response);

            foreach (var episode in jObject["seasons"])
            {
                if (string.IsNullOrWhiteSpace(episode.Value<string>("season_number")))
                    continue;

                episodes.AddRange(await this.GetEpisodesData(tvShowId, serieID, episode.Value<string>("season_number"), server.BaseUri, server.ApiKey));
            }

            return episodes;
        }

        public async Task<IEnumerable<TvShow>> Search(string term, int page = 1)
        {
            var server = this._serverRepository.GetServer();
            var shows = new List<TvShow>();
            HttpClient client = new HttpClient() { BaseAddress = new Uri(server.BaseUri) };

            var response = await client.GetStringAsync(string.Format(Uris.SearchURI, server.ApiKey, term, page));
            var jObject = JObject.Parse(response);

            foreach (var tvShow in jObject["results"])
            {
                shows.Add(new TvShow()
                {
                    ID = Guid.NewGuid(),
                    Banner = string.IsNullOrEmpty(tvShow.Value<string>("poster_path")) ? string.Empty : string.Format(Uris.ImageURI, server.ImageUri, "original", tvShow.Value<string>("poster_path")),
                    Name = tvShow.Value<string>("name"),
                    SeriesID = tvShow.Value<int>("id"),
                    Language = tvShow.Value<string>("original_language"),
                    Overview = tvShow.Value<string>("overview"),
                    FirstAired = string.IsNullOrEmpty(tvShow.Value<string>("first_air_date")) ? new DateTime(1970, 1, 1) : tvShow.Value<DateTime>("first_air_date"),
                });
            }

            return shows;
        }

        public async Task<bool> UpdateShows()
        {
            var server = this._serverRepository.GetServer();
            var hadNewEpisodes = false;
            EpisodeDTO lastEpisode;

            foreach (var serie in this._tvShowRepository.Series())
            {
                lastEpisode = this._episodeRepository.GetLastEpisodeBySeasonAndFirstAired(serie.ID);
                if (lastEpisode == null)
                    continue;

                var result = await this.SaveNewEpisodes(serie.ID, serie.SeriesID, lastEpisode.Season, server.BaseUri, server.ApiKey);
                if (!result)
                    result = await this.SaveNewEpisodes(serie.ID, serie.SeriesID, ++lastEpisode.Season, server.BaseUri, server.ApiKey);

                if (result)
                    hadNewEpisodes = true;
            }

            return hadNewEpisodes;
        }

        #endregion

        #region Private Methods

        private async Task<bool> SaveNewEpisodes(Guid serieID, int seriesID, int season, string baseUri, string apiKey)
        {
            var result = false;
            foreach (var episode in await this.GetEpisodesData(serieID, seriesID, season.ToString(), baseUri, apiKey))
            {
                var episodeDTO = this._episodeRepository.GetByEpisodeId(episode.EpisodeId);
                if (episodeDTO != null)
                {
                    episodeDTO.FirstAired = episode.FirstAired;
                    episodeDTO.Name = episode.Name;
                    this._episodeRepository.Update(episodeDTO);
                }
                else
                {
                    this._episodeRepository.Add(episode);
                    result = true;
                }
            }

            return result;
        }

        private async Task<IEnumerable<Episode>> GetEpisodesData(Guid tvShowId, int serieID, string season, string baseUri, string apiKey)
        {
            var episodes = new List<Episode>();
            HttpClient client = new HttpClient() { BaseAddress = new Uri(baseUri) };

            try
            {
                var response = await client.GetStringAsync(string.Format(Uris.EpisodesURI, serieID, season, apiKey));
                var jObject = JObject.Parse(response);

                foreach (var episode in jObject["episodes"])
                {
                    episodes.Add(new Episode()
                    {
                        ID = Guid.NewGuid(),
                        Name = episode.Value<string>("name"),
                        EpisodeId = episode.Value<int>("id"),
                        Number = episode.Value<int>("episode_number"),
                        Overview = episode.Value<string>("overview"),
                        Season = Convert.ToInt32(season),
                        TvShowId = tvShowId,
                        FirstAired = string.IsNullOrEmpty(episode.Value<string>("air_date")) ? default(DateTime?) : episode.Value<DateTime>("air_date")
                    });
                }

            }
            catch (Exception ex)
            {

            }

            return episodes;
        }

        #endregion
    }
}
