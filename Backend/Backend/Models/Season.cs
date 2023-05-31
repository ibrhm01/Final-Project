using System;
namespace Backend.Models
{
	public class Season:BaseEntity
	{
        public int SeasonNumber { get; set; }
        public List<Episode> Episodes { get; set; }
        public int TvSeriesId { get; set; }
        public TvSeries TvSeries { get; set; }


    }
}

