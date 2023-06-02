using System;
using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.ViewModels
{
	public class BannerUpdateVM
	{
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Quality { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string TeaserUrl { get; set; }
    }
}

