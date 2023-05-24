using System;
namespace Backend.Models
{
	public class Introduction:BaseEntity
	{
		public string Title { get; set; }
        public string MainPage { get; set; }
        public string CurrentPage { get; set; }

    }
}

