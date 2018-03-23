using System.Text.RegularExpressions;

namespace Labs.WPF.TorrentDownload.Model
{
    public class ShowInfo
    {
        #region Constructor

        public ShowInfo(string fileFullPath)
        {
            this.ProcessFileFullPath(fileFullPath);
        }

        #endregion

        #region Fields

        private Regex _nameRegex = new Regex(@"[A-Z]\d{2}[A-Z]\d{2}?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private Regex _nameRegex = new Regex(@"[A-Z]\d{2}[A-Z]\d{2}?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region Properties

        public string Name { get; private set; }
        public int EpisodeNumber { get; private set; }
        public int Season { get; private set; }
        public string FileName { get; private set; }
        public string FileDirectory { get; private set; }
        public string FileExtension { get; private set; }

        #endregion

        #region Private Methods

        private void ProcessFileFullPath(string fileFullPath)
        {
            
        }

        #endregion
    }
}
