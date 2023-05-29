using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class TvSeriesDetailVM
	{
        public TrialTest TrialTest { get; set; }
        public List<Category> Categories { get; set; }
        public List<TvSeries> TvSeriesList { get; set; }
        public TvSeries TvSeries { get; set; }

    }
}

