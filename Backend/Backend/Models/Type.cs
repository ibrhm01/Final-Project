using System;
namespace Backend.Models
{
	public class Type:BaseEntity
	{
		public string Name { get; set; }
		public List<UpcomingMovie> UpcomingMovies { get; set; }
	}
}

