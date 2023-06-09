using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class TopRatedController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public TopRatedController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<TopRated> TopRateds = _appDbContext.TopRateds
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.TopRateds.ToList(), take);
            PaginationVM<TopRated> pagination = new(TopRateds, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<TopRated> TopRateds, int take)
        {
            return (int)Math.Ceiling((double)TopRateds.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            TopRated TopRated = _appDbContext.TopRateds.Where(m => m.Id == id).FirstOrDefault();
            return View(TopRated);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(TopRatedCreateVM topRatedCreateVM)
        {
            if (topRatedCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (topRatedCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (topRatedCreateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (topRatedCreateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (topRatedCreateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (topRatedCreateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            
            if (!topRatedCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }
            TopRated newTopRated = new();
            List<TopRatedCategory> topRatedCategories = new();
            foreach (var item in topRatedCreateVM.CategoryIds)
            {
                TopRatedCategory TopRatedCategory = new();
                TopRatedCategory.TopRatedId = newTopRated.Id;
                TopRatedCategory.CategoryId = item;
                topRatedCategories.Add(TopRatedCategory);
            }

            newTopRated.ImageUrl = topRatedCreateVM.Photo.SaveImage(_env, "assets/img/poster", topRatedCreateVM.Photo.FileName);
            newTopRated.Name = topRatedCreateVM.Name;
            newTopRated.ReleaseDate = topRatedCreateVM.ReleaseDate;
            newTopRated.Quality = topRatedCreateVM.Quality;
            newTopRated.Duration = topRatedCreateVM.Duration;
            newTopRated.Point = topRatedCreateVM.Point;
            newTopRated.About = topRatedCreateVM.About;
            newTopRated.Status = topRatedCreateVM.Status;
            newTopRated.TeaserUrl = topRatedCreateVM.TeaserUrl;

            newTopRated.TopRatedCategories = topRatedCategories;


            _appDbContext.TopRateds.Add(newTopRated);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            TopRated existingTopRated = _appDbContext.TopRateds.Include(m => m.TopRatedCategories).FirstOrDefault(m => m.Id == id);
            if (existingTopRated == null)
            {
                return NotFound();
            }

            // Create a TopRatedEditVM object to pass the existing TopRated data to the view
            TopRatedUpdateVM topRatedUpdateVM = new TopRatedUpdateVM
            {
                Name = existingTopRated.Name,
                ReleaseDate = existingTopRated.ReleaseDate,
                Quality = existingTopRated.Quality,
                Duration = existingTopRated.Duration,
                Point = existingTopRated.Point,
                About = existingTopRated.About,
                Status = existingTopRated.Status,
                TeaserUrl = existingTopRated.TeaserUrl,

                CategoryIds = existingTopRated.TopRatedCategories.Select(mc => mc.CategoryId).ToList()
            };

            // Populate the CategoryList property of the view model with available categories
            ViewBag.Categories = new SelectList(_appDbContext.Categories.ToList(), "Id", "Name");


            return View(topRatedUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, TopRatedUpdateVM topRatedUpdateVM)
        {
            if (topRatedUpdateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (topRatedUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (topRatedUpdateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (topRatedUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (topRatedUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (topRatedUpdateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
           
            if (!topRatedUpdateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();
            }

            TopRated existingTopRated = _appDbContext.TopRateds.Include(m => m.TopRatedCategories).FirstOrDefault(m => m.Id == id);
            if (existingTopRated == null)
            {
                return NotFound();
            }

            existingTopRated.ImageUrl = topRatedUpdateVM.Photo.SaveImage(_env, "assets/img/poster", topRatedUpdateVM.Photo.FileName);
            existingTopRated.Name = topRatedUpdateVM.Name;
            existingTopRated.ReleaseDate = topRatedUpdateVM.ReleaseDate;
            existingTopRated.Quality = topRatedUpdateVM.Quality;
            existingTopRated.Duration = topRatedUpdateVM.Duration;
            existingTopRated.Point = topRatedUpdateVM.Point;
            existingTopRated.About = topRatedUpdateVM.About;
            existingTopRated.Status = topRatedUpdateVM.Status;
            existingTopRated.TeaserUrl = topRatedUpdateVM.TeaserUrl;

            _appDbContext.TopRatedCategories.RemoveRange(existingTopRated.TopRatedCategories);

            List<TopRatedCategory> topRatedCategories = new List<TopRatedCategory>();
            foreach (var categoryId in topRatedUpdateVM.CategoryIds)
            {
                TopRatedCategory TopRatedCategory = new TopRatedCategory
                {
                    TopRatedId = existingTopRated.Id,
                    CategoryId = categoryId
                };
                topRatedCategories.Add(TopRatedCategory);
            }

            existingTopRated.TopRatedCategories = topRatedCategories;

            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            TopRated deletedTopRated = _appDbContext.TopRateds.Find(id);

            if (deletedTopRated == null) return NotFound();

            _appDbContext.Remove(deletedTopRated);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

