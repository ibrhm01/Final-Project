using System;
namespace Backend.Models
{
	public class TrialTest:BaseEntity
	{
		public string Title { get; set; }
        public string Description { get; set; }
        public string Placeholder { get; set; }
        public string BackgroundImageUrl { get; set; }
    }
}

