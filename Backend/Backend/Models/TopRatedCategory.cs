using System;
namespace Backend.Models
{
	public class TopRatedCategory:BaseEntity
	{
		public int TopRatedId { get; set; }
        public TopRated TopRated { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}

