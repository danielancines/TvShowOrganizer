namespace Labs.WPF.TvShowOrganizer.Data.DTO
{
    public class TorrentInfoDTO
    {
        public TorrentInfoDTO(string name, string magnetLink, int seeders, int leechers)
        {
            this.Name = name;
            this.MagnetLink = magnetLink;
            this.Seeders = seeders;
            this.Leechers = leechers;
        }

        public int Seeders { get; private set; }
        public int Leechers { get; private set; }
        public string MagnetLink { get; private set; }
        public string Name { get; private set; }
    }
}
