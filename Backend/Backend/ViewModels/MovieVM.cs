
using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class MovieVM
	{
        public List<Movie> Movies { get; set; }
        public List<Category> Categories { get; set; }
        public TrialTest TrialTest { get; set; }

    }
}

