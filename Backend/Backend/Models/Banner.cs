using System;
namespace Backend.Models
{
	public class Banner:BaseEntity
	{
		public string BrandName { get; set; }
        public string Description { get; set; }
        public string Quality { get; set; }
        public string Status { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Duration { get; set; }
        public string TeaserUrl { get; set; }
        public List<Category> Categories { get; set; }

    }
}

