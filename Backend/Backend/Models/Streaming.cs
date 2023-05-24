using System;
namespace Backend.Models
{
	public class Streaming:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Quality { get; set; }
        public int ActiveCustomer { get; set; }

    }
}

