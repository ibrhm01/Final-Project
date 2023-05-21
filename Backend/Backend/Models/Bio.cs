using System;
namespace Backend.Models
{
	public class Bio:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Pinterest { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string FooterText { get; set; }
        public string FooterCards{ get; set; }
    }
}

