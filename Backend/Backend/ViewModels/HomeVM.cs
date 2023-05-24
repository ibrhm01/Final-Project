using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class HomeVM
	{
        public Banner Banner { get; set; }
        public List<Models.Type> Types { get; set; }
        public List<Upcoming> Upcomings { get; set; }
        public Service Service { get; set; }
        public List<BestSeries> BestSeries { get; set; }
        public TrialTest TrialTest { get; set; }
        public Streaming Streaming { get; set; }
        public List<TopRated> TopRateds{ get; set; }
        public List<Category> Categories { get; set; }

    }
}

