using HtmlAgilityPack;
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

        public Task<List<Tuple<string, string>>> GetLinks(string tvShowName, string season, string number)
        {
            if (!this._internetService.HasInternetConnection())
            {
                return null;
            }

            List<Tuple<string, string>> links = new List<Tuple<string, string>>();
            WebClient client = new WebClient();
            var task = client.DownloadStringTaskAsync(string.Format(@"https://thepiratebay.org/search/{0} s{1}e{2}", tvShowName, season, number));

            return task.ContinueWith(result =>
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(result.Result);

                var mainResultTable = doc.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "searchResult");
                if (mainResultTable is null)
                    return;

                foreach (var item in mainResultTable.Descendants("td").Where(t => t.InnerHtml.Contains("magnet")))
                {
                    var episodeName = string.Empty;
                    var magnetLink = string.Empty;
                    var nameNode = item.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("class") && a.Attributes["class"].Value == "detLink");
                    if (nameNode != null)
                        episodeName = nameNode.InnerText;

                    if (!episodeName.Contains("720p"))
                        continue;

                    var magnetLinkNode = item.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("href") && a.Attributes["href"].Value.Contains("magnet:?xt"));
                    if (magnetLinkNode != null)
                        magnetLink = magnetLinkNode.Attributes["href"].Value;

                    links.Add(new Tuple<string, string>(episodeName, magnetLink));
                }

            }).ContinueWith(l =>
            {
                return links;
            });
        } 

        #endregion
    }
}
