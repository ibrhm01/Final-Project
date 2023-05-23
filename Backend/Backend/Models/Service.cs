using System;
namespace Backend.Models
{
	public class Service:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubTitle1 { get; set; }
        public string SubDescription1 { get; set; }
        public string SubTitle2 { get; set; }
        public string SubDescription2 { get; set; }

    }
}

