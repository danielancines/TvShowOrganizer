using Labs.WPF.Core;
using Labs.WPF.Core.Handlers;
using Labs.WPF.TorrentDownload.Views;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity;
using Unity.Resolution;

namespace Labs.WPF.TorrentDownload.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor

        public MainWindowViewModel(IUnityContainer container)
        {
            this._container = container;
            this.SearchCommand = new DelegateCommand<object>(this.Execute_Search);
            this.FileNames = new ObservableCollection<string>();
            this.TvShows = new ObservableCollection<TvShow>();
        }

        #endregion

        #region Commands

        public DelegateCommand<object> SearchCommand { get; private set; }

        #endregion

        #region Fields

        private IUnityContainer _container;

        #endregion

        #region Properties

        public ObservableCollection<string> FileNames { get; set; }
        public ObservableCollection<TvShow> TvShows { get; set; }
        public string SearchTerm { get; set; }

        #endregion

        #region Private Methods

        private void Execute_Search(object obj)
        {
            var searchWindow = this._container.Resolve<SearchWindow>(new ParameterOverride("searchTerm", this.SearchTerm));

            ViewsHandler.Instance.RegisterView(searchWindow);
            searchWindow.ShowDialog();
            //searchWindow().Show();
            //System.Net.WebClient client = new System.Net.WebClient();
            //var task = client.DownloadStringTaskAsync(@"https://thepiratebay.org/search/homeland" + " s07e01");
            //task.ContinueWith(result =>
            //{
            //    var doc = new HtmlDocument();
            //    doc.LoadHtml(result.Result);

            //    var mainResultTable = doc.DocumentNode.Descendants("table").FirstOrDefault(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "searchResult");
            //    if (mainResultTable is null)
            //        return;

            //    foreach (var item in mainResultTable.Descendants("td").Where(t=>t.InnerHtml.Contains("magnet")))
            //    {
            //        var episodeName = string.Empty;
            //        var magnetLink = string.Empty;
            //        var nameNode = item.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("class") && a.Attributes["class"].Value == "detLink");
            //        if (nameNode != null)
            //            episodeName = nameNode.InnerText;

            //        var magnetLinkNode = item.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("href") && a.Attributes["href"].Value.Contains("magnet:?xt"));
            //        if (magnetLinkNode != null)
            //            magnetLink = magnetLinkNode.Attributes["href"].Value;
            //    }

            //    foreach (var nameNode in mainResultTable.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "detName"))
            //    {
            //        var linkNode = nameNode.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("class") && a.Attributes["class"].Value == "detLink");
            //        if (linkNode is null)
            //            continue;

            //        this.TvShows.Add(new TvShow() { Name = linkNode.InnerText });
            //    }
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //System.Net.WebClient client = new System.Net.WebClient();
            //var task = client.DownloadStringTaskAsync(new Uri(string.Format("http://thetvdb.com/api/GetSeries.php?seriesname={0}", this.SearchTerm)));
            //task.ContinueWith(result =>
            //{
            //    this.CreateTvShowsList(XDocument.Parse(result.Result));
            //}, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public void CreateTvShowsList(XDocument tvShowsData)
        {
            foreach (var tvShow in tvShowsData.Descendants("Series"))
            {
                var newTvShow = new TvShow() { Banner = tvShow.Element("banner") == null ? string.Empty : "http://thetvdb.com/banners/" + tvShow.Element("banner").Value, Name = tvShow.Element("SeriesName").Value };
                this.TvShows.Add(newTvShow);
            }
        }

        #endregion
    }
}
