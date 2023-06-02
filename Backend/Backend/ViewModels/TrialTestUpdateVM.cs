using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class TrialTestUpdateVM
	{
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Placeholder { get; set; }
        [Required]
        public string BackgroundImageUrl { get; set; }
    }
}

