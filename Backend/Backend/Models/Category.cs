using System;
namespace Backend.Models
{
	public class Category:BaseEntity
	{
		public string Name { get; set; }
		public int? BannerId{ get; set; }
		public Banner Banner { get; set; }
        public List<TopRatedCategory> TopRatedCategories { get; set; }
        public List<MovieCategory> MovieCategories { get; set; }
        public List<TvSeriesCategories> TvSeriesCategories { get; set; }


    }
}

