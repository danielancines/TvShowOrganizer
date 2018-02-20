using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labs.WPF.TvShowOrganizer.Data.Model
{
    public class Episode
    {
        #region Properties

        [Key]
        public Guid ID { get; set; }

        [ForeignKey("TvShow")]
        public Guid TvShowId { get; set; }

        [ForeignKey("TvShow")]
        public TvShow TvShow { get; set; }

        [Required]
        public int EpisodeId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int Season { get; set; }

        [MaxLength(1000)]
        public string Overview { get; set; }

        public DateTime? FirstAired { get; set; }

        public double LastUpdated { get; set; }

        public bool Downloaded { get; set; }

        public string TorrentURI { get; set; }

        #endregion
    }
}
