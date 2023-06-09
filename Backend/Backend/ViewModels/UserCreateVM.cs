using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
	public class UserCreateVM
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
    }
}

