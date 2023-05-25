using System;
namespace Backend.Models
{
	public class TvSeriesCategories:BaseEntity
	{
		public int TvSeriesId { get; set; }
		public TvSeries TvSeries { get; set; }
		public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

