using System;
namespace Backend.Models
{
	public class Movie:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Quality { get; set; }
        public int Duration { get; set; }
        public double Point { get; set; }
        public List<MovieCategory> MovieCategories { get; set; }

    }
}

