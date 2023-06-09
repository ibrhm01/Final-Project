using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers

{
    [Area("AdminArea")]

    public class MovieController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        public MovieController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IFileService fileService, IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
            _env = env;
            _appDbContext = appDbContext;
            _fileService = fileService;
        }
       
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Movie> movies = _appDbContext.Movies
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Movies.ToList(), take);
            PaginationVM<Movie> pagination = new(movies, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Movie> movies, int take)
        {
            return (int)Math.Ceiling((double)movies.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            Movie movie = _appDbContext.Movies.Where(m => m.Id == id).FirstOrDefault();
            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateVM movieCreateVM)
        {
            if (movieCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (movieCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (movieCreateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (movieCreateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (movieCreateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (movieCreateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            if (movieCreateVM.About == null)
            {
                ModelState.AddModelError("About", "You have to enter an About!");
                return View();
            }
            if (movieCreateVM.Status == null)
            {
                ModelState.AddModelError("Status", "You have to enter a Status!");
                return View();
            }
            if (movieCreateVM.TeaserUrl == null)
            {
                ModelState.AddModelError("TeaserUrl", "You have to enter a TeaserUrl!");
                return View();
            }
            if (!movieCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }
            Movie newMovie = new();
            List<MovieCategory> movieCategories = new();
            foreach (var item in movieCreateVM.CategoryIds)
            {
                MovieCategory movieCategory = new();
                movieCategory.MovieId = newMovie.Id;
                movieCategory.CategoryId = item;
                movieCategories.Add(movieCategory);
            }

            newMovie.ImageUrl = movieCreateVM.Photo.SaveImage(_env, "assets/img/poster", movieCreateVM.Photo.FileName);
            newMovie.Name = movieCreateVM.Name;
            newMovie.ReleaseDate = movieCreateVM.ReleaseDate;
            newMovie.Quality = movieCreateVM.Quality;
            newMovie.Duration = movieCreateVM.Duration;
            newMovie.Point = movieCreateVM.Point;
            newMovie.About = movieCreateVM.About;
            newMovie.Status = movieCreateVM.Status;
            newMovie.TeaserUrl = movieCreateVM.TeaserUrl;
            newMovie.MovieCategories = movieCategories;


            _appDbContext.Movies.Add(newMovie);
            _appDbContext.SaveChanges();


            List<AppUser> subscribedUsers = _appDbContext.Users.Where(u => u.PricingId != null).ToList();

            foreach (var subscribedUser in subscribedUsers)
            {

                string link = Url.Action("Detail", "Movie", new { Area = "", id = newMovie.Id}, Request.Scheme, Request.Host.ToString());

                string body = string.Empty;
               
                string path = "wwwroot/NewMovie.html";
                body = _fileService.ReadFile(path, body);

                body = body.Replace("{{link}}", link);
                body = body.Replace("{{MovieName}}", newMovie.Name);

                string subject = "New Movie";
                _emailService.Send(subscribedUser.Email, subject, body);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Movie existingMovie = _appDbContext.Movies.Include(m => m.MovieCategories).FirstOrDefault(m => m.Id == id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            // Create a MovieEditVM object to pass the existing movie data to the view
            MovieUpdateVM movieUpdateVM = new MovieUpdateVM
            {
                Name = existingMovie.Name,
                ReleaseDate = existingMovie.ReleaseDate,
                Quality = existingMovie.Quality,
                Duration = existingMovie.Duration,
                Point = existingMovie.Point,
                About = existingMovie.About,
                Status = existingMovie.Status,
                TeaserUrl = existingMovie.TeaserUrl,
                CategoryIds = existingMovie.MovieCategories.Select(mc => mc.CategoryId).ToList()
            };

            // Populate the CategoryList property of the view model with available categories
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");


            return View(movieUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, MovieUpdateVM movieUpdateVM)
        {
            if (movieUpdateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (movieUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (movieUpdateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (movieUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (movieUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (movieUpdateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            if (movieUpdateVM.About == null)
            {
                ModelState.AddModelError("About", "You have to enter an About!");
                return View();
            }
            if (movieUpdateVM.Status == null)
            {
                ModelState.AddModelError("Status", "You have to enter a Status!");
                return View();
            }
            if (movieUpdateVM.TeaserUrl == null)
            {
                ModelState.AddModelError("TeaserUrl", "You have to enter a TeaserUrl!");
                return View();
            }
            if (!movieUpdateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();
            }



            Movie existingMovie = _appDbContext.Movies.Include(m => m.MovieCategories).FirstOrDefault(m => m.Id == id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", existingMovie.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            existingMovie.ImageUrl = movieUpdateVM.Photo.SaveImage(_env, "assets/img/poster", movieUpdateVM.Photo.FileName);
            existingMovie.Name = movieUpdateVM.Name;
            existingMovie.ReleaseDate = movieUpdateVM.ReleaseDate;
            existingMovie.Quality = movieUpdateVM.Quality;
            existingMovie.Duration = movieUpdateVM.Duration;
            existingMovie.Point = movieUpdateVM.Point;
            existingMovie.About = movieUpdateVM.About;
            existingMovie.Status = movieUpdateVM.Status;
            existingMovie.TeaserUrl = movieUpdateVM.TeaserUrl;

            _appDbContext.MovieCategories.RemoveRange(existingMovie.MovieCategories);

            List<MovieCategory> movieCategories = new List<MovieCategory>();
            foreach (var categoryId in movieUpdateVM.CategoryIds)
            {
                MovieCategory movieCategory = new MovieCategory
                {
                    MovieId = existingMovie.Id,
                    CategoryId = categoryId
                };
                movieCategories.Add(movieCategory);
            }

            existingMovie.MovieCategories = movieCategories;

            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Movie deletedMovie = _appDbContext.Movies.Find(id);

            if (deletedMovie == null) return NotFound();


            string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", deletedMovie.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }


            _appDbContext.Remove(deletedMovie);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

