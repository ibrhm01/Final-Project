
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
	public class Pricing:BaseEntity
	{
        public string PricingType { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string VideoQuality { get; set; }
        public string Resolution { get; set; }
        public int AmountOfScreens { get; set; }
        public bool Cancel { get; set; }

    }
}

