using System;

namespace Labs.WPF.TorrentDownload.Model
{
    public class Torrent
    {
        public Torrent(Guid parentID, string name, string magnetLink)
        {
            this.ParentID = parentID;
            this.Name = name;
            this.MagnetLink = magnetLink;
        }

        public Guid ParentID { get; set; }
        public string Name { get; private set; }
        public string MagnetLink { get; private set; }
    }
}
