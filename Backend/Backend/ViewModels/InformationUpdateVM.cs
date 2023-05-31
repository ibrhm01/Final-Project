
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class InformationUpdateVM
	{
        [Required]
        public string About { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

