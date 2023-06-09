using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Backend.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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



        //public async Task<IActionResult> Edit(string id)
        //{
        //    AppUser user = await _userManager.FindByIdAsync(id);
        //    if (user == null) return NotFound();

        //    ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Id", "UserName");
        //    return View(new UserUpdateVM
        //    {
        //        User = user,
        //        UserRoles = await _userManager.GetRolesAsync(user),
        //        AllRoles = _roleManager.Roles.ToList()
        //    });
        //}

        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.AsEnumerable(), "Name", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateVM UserCreateVM)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.AsEnumerable(), "Name", "Name");


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
            if (UserCreateVM.UserRoles.Count == 0)
            {
                ModelState.AddModelError("UserRoles", "You have to choose a Role!");
                return View();
            }

            AppUser user = new();
            user.FullName = UserCreateVM.FullName;
            user.UserName = UserCreateVM.UserName;
            user.Email = UserCreateVM.Email;
            user.IsActive = true;




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

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    Movie existingMovie = _appDbContext.Movies.Include(m => m.MovieCategories).FirstOrDefault(m => m.Id == id);
        //    if (existingMovie == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a MovieEditVM object to pass the existing movie data to the view
        //    MovieUpdateVM movieUpdateVM = new MovieUpdateVM
        //    {
        //        Name = existingMovie.Name,
        //        ReleaseDate = existingMovie.ReleaseDate,
        //        Quality = existingMovie.Quality,
        //        Duration = existingMovie.Duration,
        //        Point = existingMovie.Point,
        //        About = existingMovie.About,
        //        Status = existingMovie.Status,
        //        TeaserUrl = existingMovie.TeaserUrl,
        //        CategoryIds = existingMovie.MovieCategories.Select(mc => mc.CategoryId).ToList()
        //    };

        //    // Populate the CategoryList property of the view model with available categories
        //    ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");


        //    return View(movieUpdateVM);
        //}




        //[HttpPost]
        //public IActionResult Edit(int id, MovieUpdateVM movieUpdateVM)
        //{
        //    if (movieUpdateVM.Photo == null)
        //    {
        //        ModelState.AddModelError("Photo", "You have to submit a file!");
        //        return View();
        //    }
        //    if (movieUpdateVM.Name == null)
        //    {
        //        ModelState.AddModelError("Name", "You have to enter a Name!");
        //        return View();
        //    }
        //    if (movieUpdateVM.ReleaseDate == null)
        //    {
        //        ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
        //        return View();
        //    }
        //    if (movieUpdateVM.Quality == null)
        //    {
        //        ModelState.AddModelError("Quality", "You have to enter a Quality!");
        //        return View();
        //    }
        //    if (movieUpdateVM.Duration == null)
        //    {
        //        ModelState.AddModelError("Duration", "You have to enter a Duration!");
        //        return View();
        //    }
        //    if (movieUpdateVM.Point == null)
        //    {
        //        ModelState.AddModelError("Point", "You have to enter a Rating!");
        //        return View();
        //    }
        //    if (movieUpdateVM.About == null)
        //    {
        //        ModelState.AddModelError("About", "You have to enter an About!");
        //        return View();
        //    }
        //    if (movieUpdateVM.Status == null)
        //    {
        //        ModelState.AddModelError("Status", "You have to enter a Status!");
        //        return View();
        //    }
        //    if (movieUpdateVM.TeaserUrl == null)
        //    {
        //        ModelState.AddModelError("TeaserUrl", "You have to enter a TeaserUrl!");
        //        return View();
        //    }
        //    if (!movieUpdateVM.Photo.IsImage())
        //    {
        //        ModelState.AddModelError("Photo", "You have to upload an image!");
        //        return View();
        //    }



        //    Movie existingMovie = _appDbContext.Movies.Include(m => m.MovieCategories).FirstOrDefault(m => m.Id == id);
        //    if (existingMovie == null)
        //    {
        //        return NotFound();
        //    }

        //    string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", existingMovie.ImageUrl);

        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        System.IO.File.Delete(fullPath);
        //    }

        //    existingMovie.ImageUrl = movieUpdateVM.Photo.SaveImage(_env, "assets/img/poster", movieUpdateVM.Photo.FileName);
        //    existingMovie.Name = movieUpdateVM.Name;
        //    existingMovie.ReleaseDate = movieUpdateVM.ReleaseDate;
        //    existingMovie.Quality = movieUpdateVM.Quality;
        //    existingMovie.Duration = movieUpdateVM.Duration;
        //    existingMovie.Point = movieUpdateVM.Point;
        //    existingMovie.About = movieUpdateVM.About;
        //    existingMovie.Status = movieUpdateVM.Status;
        //    existingMovie.TeaserUrl = movieUpdateVM.TeaserUrl;

        //    _appDbContext.MovieCategories.RemoveRange(existingMovie.MovieCategories);

        //    List<MovieCategory> movieCategories = new List<MovieCategory>();
        //    foreach (var categoryId in movieUpdateVM.CategoryIds)
        //    {
        //        MovieCategory movieCategory = new MovieCategory
        //        {
        //            MovieId = existingMovie.Id,
        //            CategoryId = categoryId
        //        };
        //        movieCategories.Add(movieCategory);
        //    }

        //    existingMovie.MovieCategories = movieCategories;

        //    _appDbContext.SaveChanges();

        //    return RedirectToAction("Index");
        //}


        //public IActionResult Delete(int id)
        //{
        //    if (id == null) return NotFound();

        //    Movie deletedMovie = _appDbContext.Movies.Find(id);

        //    if (deletedMovie == null) return NotFound();


        //    string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", deletedMovie.ImageUrl);

        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        System.IO.File.Delete(fullPath);
        //    }


        //    _appDbContext.Remove(deletedMovie);
        //    _appDbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}

