﻿using System;
namespace Backend.Models
{
	public class TvSeries:BaseEntity
	{
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Quality { get; set; }
        public int Duration { get; set; }
        public double Point { get; set; }
        public string TeaserUrl { get; set; }
        public string Status { get; set; }
        public string About { get; set; }
        public List<TvSeriesCategories> TvSeriesCategories { get; set; }
        public List<Season> Seasons { get; set; }

    }
}

