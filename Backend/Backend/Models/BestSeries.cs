using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class BestSeries:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Quality { get; set; }
        public int Duration { get; set; }
        public double Point { get; set; }
    }
}

