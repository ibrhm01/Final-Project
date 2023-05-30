using System;
using Microsoft.AspNetCore.Identity;

namespace Backend.Helpers
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"'{userName}' artıq mövcuddur"
            };
        }


    }
}

