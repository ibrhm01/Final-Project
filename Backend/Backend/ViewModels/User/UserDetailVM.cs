using System;
using Backend.Models;

namespace Backend.ViewModels.User
{
    public class UserDetailVM
    {
        public AppUser User { get; set; }
        public IList<string> UserRoles{get; set;}
    }
}

