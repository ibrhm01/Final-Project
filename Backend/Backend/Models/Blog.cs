using System;
namespace Backend.Models
{
	public class Blog:BaseEntity
	{
		public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Like { get; set; }

    }
}

