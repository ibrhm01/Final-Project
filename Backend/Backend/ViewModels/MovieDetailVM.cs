using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class MovieDetailVM
	{
        public Movie Movie { get; set; }
        public List<Category> Categories { get; set; }
        public TrialTest TrialTest { get; set; }
        public List<Movie> Movies { get; set; }

    }
}

