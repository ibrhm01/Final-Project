using System;
namespace Backend.Models
{
	public class BaseEntity
	{
		public int ID { get; set; }
        public bool IsDeleted { get; set; }
    }
}

