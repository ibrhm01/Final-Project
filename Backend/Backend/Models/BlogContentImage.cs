using System;
namespace Backend.Models
{
	public class BlogContentImage:BaseEntity
	{
		public string ImageUrl { get; set; }
		public int BlogId { get; set; }
		public Blog Blog { get; set; }
	}
}

