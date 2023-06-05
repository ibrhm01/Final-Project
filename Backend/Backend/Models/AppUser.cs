﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        //public List<Comment> Comments { get; set; }
    }
}


