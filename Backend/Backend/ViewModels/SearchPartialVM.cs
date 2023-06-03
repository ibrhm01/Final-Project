using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class SearchPartialVM
	{
		public List<Movie> Movies { get; set; }
        public List<TvSeries> TvSeries { get; set; }
        public List<Blog> Blogs { get; set; }

    }
}

