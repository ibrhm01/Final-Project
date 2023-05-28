
using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class TvSeriesVM
	{
        public TrialTest TrialTest { get; set; }
        public List<Category> Categories{ get; set; }
        public List<TvSeries> TvSeries { get; set; }


    }
}

