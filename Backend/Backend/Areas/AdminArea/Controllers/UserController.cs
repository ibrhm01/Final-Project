using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Migrations;
using Backend.Models;
using Backend.ViewModels;
using Backend.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;


        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
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


        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.AsEnumerable(), "Name", "Name");
            ViewBag.Pricings = new SelectList(_appDbContext.Pricings.ToList(), "Id", "PricingType");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateVM UserCreateVM)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.AsEnumerable(), "Name", "Name");
            ViewBag.Pricings = new SelectList(_appDbContext.Pricings.ToList(), "Id", "PricingType");


            if (UserCreateVM.FullName == null)
            {
                ModelState.AddModelError("FullName", "You have to enter a FullName!");
                return View();
            }

            if (UserCreateVM.UserName == null)
            {
                ModelState.AddModelError("UserName", "You have to enter a UserName!");
                return View();
            }

            if (UserCreateVM.Email == null)
            {
                ModelState.AddModelError("Email", "You have to enter a Email!");
                return View();
            }
            if (UserCreateVM.IsActive == null)
            {
                ModelState.AddModelError("IsActive", "You have to enter a IsActive!");
                return View();
            }
            if (UserCreateVM.EmailConfirmed == null)
            {
                ModelState.AddModelError("EmailConfirmed", "You have to enter an EmailConfirmed!");
                return View();
            }
            if (UserCreateVM.UserRoles.Count == 0)
            {
                ModelState.AddModelError("UserRoles", "You have to choose a Role!");
                return View();
            }
            foreach (var item in _userManager.Users)
            {
                if (UserCreateVM.UserName == item.UserName|| UserCreateVM.Email == item.Email)
                {
                    ModelState.AddModelError("", "There is already such user!");
                    return View();
                }
            }

            

            AppUser user = new();
            user.FullName = UserCreateVM.FullName;
            user.UserName = UserCreateVM.UserName;
            user.Email = UserCreateVM.Email;
            user.IsActive = UserCreateVM.IsActive;
            user.EmailConfirmed = UserCreateVM.EmailConfirmed;
            user.PricingId = UserCreateVM.PricingId;

            

            IdentityResult result1 = await _userManager.CreateAsync(user, UserCreateVM.Password);

            if (!result1.Succeeded)
            {
                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(UserCreateVM);

            }

            IdentityResult result2 = await _userManager.AddToRolesAsync(user, UserCreateVM.UserRoles);



            if (!result2.Succeeded)
            {
                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(UserCreateVM);
            }



            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            AppUser existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(existingUser, "SuperAdmin"))
            {
                return RedirectToAction("Index");
            }


            UserUpdateVM userUpdateVM = new UserUpdateVM
            {
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                UserName = existingUser.UserName,
                IsActive = existingUser.IsActive,
                EmailConfirmed = existingUser.EmailConfirmed,
                UserRoles = await _userManager.GetRolesAsync(existingUser),
                PricingId = existingUser.PricingId
            };

            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            ViewBag.Pricings = new SelectList(_appDbContext.Pricings.ToList(), "Id", "PricingType");



            return View(userUpdateVM);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserUpdateVM userUpdateVM)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            ViewBag.Pricings = new SelectList(_appDbContext.Pricings.ToList(), "Id", "PricingType");


            if (userUpdateVM.FullName == null)
            {
                ModelState.AddModelError("FullName", "You have to enter a FullName!");
                return View();
            }

            if (userUpdateVM.Email == null)
            {
                ModelState.AddModelError("Email", "You have to enter an Email!");
                return View();
            }
            if (userUpdateVM.UserName == null)
            {
                ModelState.AddModelError("UserName", "You have to enter a UserName!");
                return View();
            }
            if (userUpdateVM.Password == null)
            {
                ModelState.AddModelError("Password", "You have to enter a Password!");
                return View();
            }
            if (userUpdateVM.IsActive == null)
            {
                ModelState.AddModelError("IsActive", "You have to enter a IsActive!");
                return View();
            }
            if (userUpdateVM.EmailConfirmed == null)
            {
                ModelState.AddModelError("EmailConfirmed", "You have to enter an EmailConfirmed!");
                return View();
            }
            if (userUpdateVM.UserRoles.Count == 0)
            {
                ModelState.AddModelError("UserRoles", "You have to choose a UserRole!");
                return View();
            }

            AppUser existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

           

            existingUser.FullName = userUpdateVM.FullName;
            existingUser.Email = userUpdateVM.Email;
            existingUser.UserName = userUpdateVM.UserName;
            existingUser.IsActive = userUpdateVM.IsActive;
            existingUser.EmailConfirmed = userUpdateVM.EmailConfirmed;
            existingUser.PricingId = userUpdateVM.PricingId;


            await _userManager.RemoveFromRolesAsync(existingUser, await _userManager.GetRolesAsync(existingUser));

            await _userManager.AddToRolesAsync(existingUser, userUpdateVM.UserRoles);

            var result1 = await _userManager.UpdateAsync(existingUser);

            if (!result1.Succeeded)
            {
                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result2 = await _userManager.ResetPasswordAsync(existingUser, token, userUpdateVM.Password);


            if (!result2.Succeeded)
            {
                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            AppUser deletedUser = await _userManager.FindByIdAsync(id);



            if (deletedUser == null) return NotFound();

            if(await _userManager.IsInRoleAsync(deletedUser, "SuperAdmin"))
            {
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(deletedUser);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction("Index");
        }
    }
}

