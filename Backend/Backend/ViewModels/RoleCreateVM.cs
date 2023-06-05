using System;
using Microsoft.AspNetCore.Identity;

namespace Backend.ViewModels
{
	public class RoleCreateVM
	{
		public IdentityRole Role { get; set; }
        public string RoleName { get; set; }

    }
}

