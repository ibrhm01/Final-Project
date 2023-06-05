using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class CategoryUpdateVM
	{
        [Required]
        public string Name { get; set; }

        public int? BannerId { get; set; }
        [MinLength(0, ErrorMessage = "At least one item must be selected.")]
        public List<int>? MovieIds { get; set; }
        [MinLength(0, ErrorMessage = "At least one item must be selected.")]
        public List<int>? TopRatedIds { get; set; }
        [MinLength(0, ErrorMessage = "At least one item must be selected.")]
        public List<int>? TvSeriesIds { get; set; }
    }
}

