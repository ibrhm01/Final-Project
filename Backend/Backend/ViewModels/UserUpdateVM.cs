using System;
using System.ComponentModel.DataAnnotations;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.ViewModels
{
	public class UserUpdateVM
	{
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public IList<string> UserRoles { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int? PricingId { get; set; }


    }
}

