using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class HomeVM
	{
        public Banner Banner { get; set; }
        public List<Models.Type> Types { get; set; }
        public List<UpcomingMovie> UpcomingMovies { get; set; }
        public Service Service { get; set; }
        public List<BestSeries> BestSeries { get; set; }
        public TrialTest TrialTest { get; set; }

    }
}

