using System;
namespace Backend.Models
{
	public class Information:BaseEntity
	{
        public string About { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}

