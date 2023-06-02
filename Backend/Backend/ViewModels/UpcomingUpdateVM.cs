﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class UpcomingUpdateVM
	{
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Quality { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Point { get; set; }
        [Required]
        public int TypeId { get; set; }
    }
}

