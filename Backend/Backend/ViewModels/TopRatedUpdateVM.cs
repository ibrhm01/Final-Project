using System;
using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.ViewModels
{
	public class TopRatedUpdateVM
	{
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Quality { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Point { get; set; }
        [Required]
        public string TeaserUrl { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string About { get; set; }
        [Required]
        public List<int> CategoryIds { get; set; }

        public List<Category> CategoryList { get; set; }

    }
}

