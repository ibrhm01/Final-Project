using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class StreamingUpdateVM
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
        public string Quality { get; set; }
        [Required]
        public int ActiveCustomer { get; set; }
    }
}

