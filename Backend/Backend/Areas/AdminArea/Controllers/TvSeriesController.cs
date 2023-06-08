using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
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

    public class TvSeriesController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;


        public TvSeriesController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IFileService fileService, IWebHostEnvironment env, AppDbContext appDbContext)
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
            List<TvSeries> tvSeries = _appDbContext.TvSeries
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.TvSeries.ToList(), take);
            PaginationVM<TvSeries> pagination = new(tvSeries, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<TvSeries> tvSeries, int take)
        {
            return (int)Math.Ceiling((double)tvSeries.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            TvSeries tvSeries = _appDbContext.TvSeries.Where(m => m.Id == id).FirstOrDefault();
            return View(tvSeries);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(TvSeriesCreateVM tvSeriesCreateVM)
        {
            if (tvSeriesCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (tvSeriesCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (tvSeriesCreateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (tvSeriesCreateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (tvSeriesCreateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (tvSeriesCreateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            if (tvSeriesCreateVM.About == null)
            {
                ModelState.AddModelError("About", "You have to enter an About!");
                return View();
            }
            if (tvSeriesCreateVM.Status == null)
            {
                ModelState.AddModelError("Status", "You have to enter a Status!");
                return View();
            }
            if (tvSeriesCreateVM.TeaserUrl == null)
            {
                ModelState.AddModelError("TeaserUrl", "You have to enter a TeaserUrl!");
                return View();
            }
            if (!tvSeriesCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }
            TvSeries newTvSeries = new();
            List<TvSeriesCategories> tvSeriesCategories = new();
            foreach (var item in tvSeriesCreateVM.CategoryIds)
            {
                TvSeriesCategories tvSeriesCategory= new();
                tvSeriesCategory.TvSeriesId = newTvSeries.Id;
                tvSeriesCategory.CategoryId = item;
                tvSeriesCategories.Add(tvSeriesCategory);
            }

            newTvSeries.ImageUrl = tvSeriesCreateVM.Photo.SaveImage(_env, "assets/img/poster", tvSeriesCreateVM.Photo.FileName);
            newTvSeries.Name = tvSeriesCreateVM.Name;
            newTvSeries.ReleaseDate = tvSeriesCreateVM.ReleaseDate;
            newTvSeries.Quality = tvSeriesCreateVM.Quality;
            newTvSeries.Duration = tvSeriesCreateVM.Duration;
            newTvSeries.Point = tvSeriesCreateVM.Point;
            newTvSeries.About = tvSeriesCreateVM.About;
            newTvSeries.Status = tvSeriesCreateVM.Status;
            newTvSeries.TeaserUrl = tvSeriesCreateVM.TeaserUrl;
            newTvSeries.TvSeriesCategories = tvSeriesCategories;


            _appDbContext.TvSeries.Add(newTvSeries);
            _appDbContext.SaveChanges();

            List<AppUser> subscribedUsers = _appDbContext.Users.Where(u => u.PricingId != null).ToList();

            foreach (var subscribedUser in subscribedUsers)
            {

                string link = Url.Action("Detail", "TvSeries", new { Area = "", id = newTvSeries.Id }, Request.Scheme, Request.Host.ToString());

                string body = string.Empty;

                string path = "wwwroot/NewTvSeries.html";
                body = _fileService.ReadFile(path, body);

                body = body.Replace("{{link}}", link);
                body = body.Replace("{{TvSeriesName}}", newTvSeries.Name);

                string subject = "New TvSeries";
                _emailService.Send(subscribedUser.Email, subject, body);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            TvSeries existingTvSeries = _appDbContext.TvSeries.Include(m => m.TvSeriesCategories).FirstOrDefault(m => m.Id == id);
            if (existingTvSeries == null)
            {
                return NotFound();
            }

            // Create a TvSeriesEditVM object to pass the existing TvSeries data to the view
            TvSeriesUpdateVM tvSeriesUpdateVM = new TvSeriesUpdateVM
            {
                Name = existingTvSeries.Name,
                ReleaseDate = existingTvSeries.ReleaseDate,
                Quality = existingTvSeries.Quality,
                Duration = existingTvSeries.Duration,
                Point = existingTvSeries.Point,
                About = existingTvSeries.About,
                Status = existingTvSeries.Status,
                TeaserUrl = existingTvSeries.TeaserUrl,
                CategoryIds = existingTvSeries.TvSeriesCategories.Select(mc => mc.CategoryId).ToList()
            };

            // Populate the CategoryList property of the view model with available categories
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");


            return View(tvSeriesUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, TvSeriesUpdateVM tvSeriesUpdateVM)
        {
            if (tvSeriesUpdateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (tvSeriesUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (tvSeriesUpdateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (tvSeriesUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (tvSeriesUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (tvSeriesUpdateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            if (tvSeriesUpdateVM.About == null)
            {
                ModelState.AddModelError("About", "You have to enter an About!");
                return View();
            }
            if (tvSeriesUpdateVM.Status == null)
            {
                ModelState.AddModelError("Status", "You have to enter a Status!");
                return View();
            }
            if (tvSeriesUpdateVM.TeaserUrl == null)
            {
                ModelState.AddModelError("TeaserUrl", "You have to enter a TeaserUrl!");
                return View();
            }
            if (!tvSeriesUpdateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();
            }

            TvSeries existingTvSeries = _appDbContext.TvSeries.Include(m => m.TvSeriesCategories).FirstOrDefault(m => m.Id == id);
            if (existingTvSeries == null)
            {
                return NotFound();
            }

            string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", existingTvSeries.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            existingTvSeries.ImageUrl = tvSeriesUpdateVM.Photo.SaveImage(_env, "assets/img/poster", tvSeriesUpdateVM.Photo.FileName);
            existingTvSeries.Name = tvSeriesUpdateVM.Name;
            existingTvSeries.ReleaseDate = tvSeriesUpdateVM.ReleaseDate;
            existingTvSeries.Quality = tvSeriesUpdateVM.Quality;
            existingTvSeries.Duration = tvSeriesUpdateVM.Duration;
            existingTvSeries.Point = tvSeriesUpdateVM.Point;
            existingTvSeries.About = tvSeriesUpdateVM.About;
            existingTvSeries.Status = tvSeriesUpdateVM.Status;
            existingTvSeries.TeaserUrl = tvSeriesUpdateVM.TeaserUrl;

            _appDbContext.TvSeriesCategories.RemoveRange(existingTvSeries.TvSeriesCategories);

            List<TvSeriesCategories> tvSeriesCategories = new List<TvSeriesCategories>();
            foreach (var categoryId in tvSeriesUpdateVM.CategoryIds)
            {
                TvSeriesCategories TvSeriesCategory = new TvSeriesCategories
                {
                    TvSeriesId = existingTvSeries.Id,
                    CategoryId = categoryId
                };
                tvSeriesCategories.Add(TvSeriesCategory);
            }

            existingTvSeries.TvSeriesCategories = tvSeriesCategories;

            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            TvSeries deletedTvSeries = _appDbContext.TvSeries.Find(id);

            if (deletedTvSeries == null) return NotFound();

            _appDbContext.Remove(deletedTvSeries);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

