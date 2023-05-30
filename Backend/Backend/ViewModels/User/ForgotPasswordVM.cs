using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels.User
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

