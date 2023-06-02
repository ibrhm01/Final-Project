using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class TopRatedDetailVM
	{
        public TopRated TopRated { get; set; }
        public List<Category> Categories { get; set; }
        public TrialTest TrialTest { get; set; }
        public List<TopRated> TopRateds { get; set; }
    }
}

