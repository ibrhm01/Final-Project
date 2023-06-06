using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<AppUser> users = _userManager.Users
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_userManager.Users.ToList(), take);
            PaginationVM<AppUser> pagination = new(users, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<AppUser> users, int take)
        {
            return (int)Math.Ceiling((double)users.Count / (double)take);
        }

        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            return View(new UserDetailVM
            {
                User = user,
                UserRoles = userRoles
            });
        }



    }
}

