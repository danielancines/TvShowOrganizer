using System;

namespace Labs.WPF.TorrentDownload.Model
{
    public class Torrent
    {
        public Torrent(Guid parentID, string name, string magnetLink, int seeders, int leechers)
        {
            this.ParentID = parentID;
            this.Name = name;
            this.MagnetLink = magnetLink;
            this.Seeders = seeders;
            this.Leechers = leechers;
        }

        public Guid ParentID { get; set; }
        public string Name { get; private set; }
        public string MagnetLink { get; private set; }
        public int Seeders { get; private set; }
        public int Leechers { get; private set; }
    }
}
