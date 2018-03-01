using HtmlAgilityPack;
using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class TorrentService : ITorrentService
    {
        #region Constructor

        public TorrentService(IInternetService internetService)
        {
            _internetService = internetService;
        }

        #endregion

        #region Fields

        private IInternetService _internetService;

        #endregion

        #region ITorrentService Members

        public Task<List<TorrentInfoDTO>> GetLinks(string tvShowName, string season, string number)
        {
            if (!this._internetService.HasInternetConnection())
            {
                return null;
            }

            List<TorrentInfoDTO> links = new List<TorrentInfoDTO>();
            WebClient client = new WebClient();
            var task = client.DownloadStringTaskAsync(string.Format(@"https://thepiratebay.org/search/{0} s{1}e{2}", tvShowName, season, number));

            return task.ContinueWith(result =>
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(result.Result);

                var mainResultTable = doc.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "searchResult");
                if (mainResultTable is null)
                    return;

                foreach (var item in mainResultTable.Descendants("tr").Where(t => t.InnerHtml.Contains("magnet")))
                {
                    var episodeInfo = mainResultTable.Descendants("td").FirstOrDefault(t => t.InnerHtml.Contains("magnet"));
                    if (episodeInfo == null)
                        continue;

                    int seeders = 0, leechers = 0;
                    var seedersInfo = item.Descendants("td").Where(x => x.Attributes.Contains("align")).FirstOrDefault();
                    var leechersInfo = item.Descendants("td").Where(x => x.Attributes.Contains("align")).LastOrDefault();
                    if (seedersInfo != null)
                        seeders = Convert.ToInt32(seedersInfo.InnerText);
                    if (leechersInfo != null)
                        leechers = Convert.ToInt32(leechersInfo.InnerText);

                    var episodeName = string.Empty;
                    var magnetLink = string.Empty;
                    var nameNode = item.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("class") && a.Attributes["class"].Value == "detLink");
                    if (nameNode != null)
                        episodeName = nameNode.InnerText;

                    if (!episodeName.Contains("720p"))
                        continue;

                    var magnetLinkNode = episodeInfo.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("href") && a.Attributes["href"].Value.Contains("magnet:?xt"));
                    if (magnetLinkNode != null)
                        magnetLink = magnetLinkNode.Attributes["href"].Value;

                    links.Add(new TorrentInfoDTO(episodeName, magnetLink, seeders, leechers));
                }
            }).ContinueWith(l =>
            {
                return links;
            });
        }

        #endregion
    }
}
