using System;
using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.ViewModels
{
	public class SeasonCreateVM
	{
        [Required]
        public int SeasonNumber { get; set; }
        [Required]
        public int TvSeriesId { get; set; }
    }
}

