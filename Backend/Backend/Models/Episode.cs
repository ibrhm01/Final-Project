using System;
namespace Backend.Models
{
	public class Episode:BaseEntity
	{

		public string Name { get; set; }
        public int Duration { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }

    }
}

