using System;
using System.ComponentModel.DataAnnotations;

namespace Labs.WPF.TvShowOrganizer.Data.Model
{
    public class Server
    {
        [Key]
        public Guid ID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string BaseUri { get; set; }

        [Required, MaxLength(50)]
        public string UpdateUri { get; set; }

        public double LastUpdate { get; set; }
    }
}
