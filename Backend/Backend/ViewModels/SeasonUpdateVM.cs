using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class SeasonUpdateVM
    {
        [Required]
        public int SeasonNumber { get; set; }
        [Required]
        public int TvSeriesId { get; set; }

    }
}

