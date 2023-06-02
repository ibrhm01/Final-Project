using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class ServiceUpdateVM
	{
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SubTitle1 { get; set; }
        [Required]
        public string SubDescription1 { get; set; }
        [Required]
        public string SubTitle2 { get; set; }
        [Required]
        public string SubDescription2 { get; set; }
    }
}

