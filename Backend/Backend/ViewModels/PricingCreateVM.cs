using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Backend.ViewModels
{
	public class PricingCreateVM
	{
        [Required]
        public string PricingType { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string VideoQuality { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        public int AmountOfScreens { get; set; }
        [Required]
        public bool Cancel { get; set; }
    }
}

