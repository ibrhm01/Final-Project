using System;
namespace Backend.ViewModels
{
	public class EpisodeUpdateVM
	{
        public string Name { get; set; }
        public int EpisodeNumber { get; set; }
        public int Duration { get; set; }
        public int SeasonId { get; set; }
    }
}

