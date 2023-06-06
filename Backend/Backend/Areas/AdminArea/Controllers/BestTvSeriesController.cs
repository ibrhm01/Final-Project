using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class BestTvSeriesController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public BestTvSeriesController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<BestSeries> BestSeries = _appDbContext.BestSeries
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.BestSeries.ToList(), take);
            PaginationVM<BestSeries> pagination = new(BestSeries, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<BestSeries> BestSeries, int take)
        {
            return (int)Math.Ceiling((double)BestSeries.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            BestSeries BestSeries = _appDbContext.BestSeries.Where(m => m.Id == id).FirstOrDefault();
            return View(BestSeries);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(BestSeriesCreateVM BestSeriesCreateVM)
        {
            if (BestSeriesCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (BestSeriesCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (BestSeriesCreateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (BestSeriesCreateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (BestSeriesCreateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (BestSeriesCreateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
           
            if (!BestSeriesCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }
            BestSeries newBestSeries = new();
           

            newBestSeries.ImageUrl = BestSeriesCreateVM.Photo.SaveImage(_env, "assets/img/poster", BestSeriesCreateVM.Photo.FileName);
            newBestSeries.Name = BestSeriesCreateVM.Name;
            newBestSeries.ReleaseDate = BestSeriesCreateVM.ReleaseDate;
            newBestSeries.Quality = BestSeriesCreateVM.Quality;
            newBestSeries.Duration = BestSeriesCreateVM.Duration;
            newBestSeries.Point = BestSeriesCreateVM.Point;
            


            _appDbContext.BestSeries.Add(newBestSeries);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            BestSeries existingBestSeries = _appDbContext.BestSeries.FirstOrDefault(m => m.Id == id);
            if (existingBestSeries == null)
            {
                return NotFound();
            }

            // Create a BestSeriesEditVM object to pass the existing BestSeries data to the view
            BestSeriesUpdateVM BestSeriesUpdateVM = new BestSeriesUpdateVM
            {
                Name = existingBestSeries.Name,
                ReleaseDate = existingBestSeries.ReleaseDate,
                Quality = existingBestSeries.Quality,
                Duration = existingBestSeries.Duration,
                Point = existingBestSeries.Point,
                
            };

            // Populate the CategoryList property of the view model with available categories


            return View(BestSeriesUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, BestSeriesUpdateVM BestSeriesUpdateVM)
        {
            if (BestSeriesUpdateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (BestSeriesUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (BestSeriesUpdateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (BestSeriesUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (BestSeriesUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (BestSeriesUpdateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Rating!");
                return View();
            }
            
            if (!BestSeriesUpdateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();
            }

            BestSeries existingBestSeries = _appDbContext.BestSeries.FirstOrDefault(m => m.Id == id);
            if (existingBestSeries == null)
            {
                return NotFound();
            }

            string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", existingBestSeries.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            existingBestSeries.ImageUrl = BestSeriesUpdateVM.Photo.SaveImage(_env, "assets/img/poster", BestSeriesUpdateVM.Photo.FileName);
            existingBestSeries.Name = BestSeriesUpdateVM.Name;
            existingBestSeries.ReleaseDate = BestSeriesUpdateVM.ReleaseDate;
            existingBestSeries.Quality = BestSeriesUpdateVM.Quality;
            existingBestSeries.Duration = BestSeriesUpdateVM.Duration;
            existingBestSeries.Point = BestSeriesUpdateVM.Point;
         
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            BestSeries deletedBestSeries = _appDbContext.BestSeries.Find(id);

            if (deletedBestSeries == null) return NotFound();

            _appDbContext.Remove(deletedBestSeries);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

