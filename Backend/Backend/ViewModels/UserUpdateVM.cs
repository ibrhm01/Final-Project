using System;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.ViewModels
{
	public class UserUpdateVM
	{
		public AppUser User { get; set; }
        public IList<string> UserRoles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }


    }
}

