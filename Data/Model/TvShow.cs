using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labs.WPF.TvShowOrganizer.Data.Model
{
    [Table("TvShows")]
    public class TvShow
    {
        #region Constructor

        public TvShow()
        {
            this.ID = new Guid();
        }

        #endregion

        #region Properties

        [Key]
        public Guid ID { get; set; }

        [Required]
        public int SeriesID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(5)]
        public string Language { get; set; }

        [MaxLength(200)]
        public string Banner { get; set; }

        [MaxLength(500)]
        public string Overview { get; set; }

        [MaxLength(10)]
        public string Network { get; set; }

        [MaxLength(20)]
        public string ImdbId { get; set; }

        [MaxLength(20)]
        public string DatabaseId { get; set; }

        public DateTime? FirstAired { get; set; }

        public double LastUpdated { get; set; }

        [ForeignKey("Episode")]
        public virtual ICollection<Episode> Episodes { get; set; }

        #endregion
    }
}
