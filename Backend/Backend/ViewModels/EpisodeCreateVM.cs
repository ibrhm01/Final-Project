using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class EpisodeCreateVM
	{
        [Required]
        public string Name { get; set; }
        [Required]
        public int EpisodeNumber { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int SeasonId { get; set; }
    }
}

