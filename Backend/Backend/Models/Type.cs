﻿using System;
namespace Backend.Models
{
	public class Type:BaseEntity
	{
		public string Name { get; set; }
		public List<Upcoming> Upcomings { get; set; }
	}
}

