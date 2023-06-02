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

    public class UpcomingController : Controller
    {
        // GET: /<controller>/
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public UpcomingController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Upcoming> upcomings = _appDbContext.Upcomings
                .Skip((page - 1) * take)
                .Take(take)
                .Include(u=>u.Type)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Upcomings.ToList(), take);
            PaginationVM<Upcoming> pagination = new(upcomings, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Upcoming> upcomings, int take)
        {
            return (int)Math.Ceiling((double)upcomings.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            Upcoming Upcoming = _appDbContext.Upcomings.Where(m => m.Id == id).Include(u => u.Type).FirstOrDefault();
            return View(Upcoming);
        }

        public IActionResult Create()
        {
            ViewBag.Types = new SelectList(_appDbContext.Types.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(UpcomingCreateVM upcomingCreateVM)
        {
            if (upcomingCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            
            if (upcomingCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (upcomingCreateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (upcomingCreateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (upcomingCreateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (upcomingCreateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Point!");
                return View();
            }
            if (upcomingCreateVM.TypeId == 0)
            {
                ModelState.AddModelError("TypeId", "You have to enter a TypeId!");
                return View();
            }

            if (!upcomingCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }



            Upcoming newUpcoming = new();
            newUpcoming.ImageUrl = upcomingCreateVM.Photo.SaveImage(_env, "assets/img/poster", upcomingCreateVM.Photo.FileName);
            newUpcoming.Name = upcomingCreateVM.Name;
            newUpcoming.Quality = upcomingCreateVM.Quality;
            newUpcoming.Duration = upcomingCreateVM.Duration;
            newUpcoming.Point = upcomingCreateVM.Point;
            newUpcoming.ReleaseDate = upcomingCreateVM.ReleaseDate;
            newUpcoming.TypeId = upcomingCreateVM.TypeId;


            _appDbContext.Upcomings.Add(newUpcoming);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            ViewBag.Types = new SelectList(_appDbContext.Types.ToList(), "Id", "Name");


            Upcoming updatedUpcoming = _appDbContext.Upcomings.Find(id);

            if (updatedUpcoming == null) return NotFound();

            return View(new UpcomingUpdateVM
            {
                Name = updatedUpcoming.Name,
                ImageUrl = updatedUpcoming.ImageUrl,
                ReleaseDate = updatedUpcoming.ReleaseDate,
                Duration = updatedUpcoming.Duration,
                Quality = updatedUpcoming.Quality,
                Point = updatedUpcoming.Point,
                TypeId = updatedUpcoming.TypeId,

            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, UpcomingUpdateVM upcomingUpdateVM)
        {
            if (id == null) return NotFound();

            Upcoming updatedUpcoming = _appDbContext.Upcomings.Find(id);

            if (updatedUpcoming == null) return NotFound();


            if (upcomingUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            if (upcomingUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter a Quality!");
                return View();
            }
            if (upcomingUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter a Duration!");
                return View();
            }
            if (upcomingUpdateVM.ReleaseDate == null)
            {
                ModelState.AddModelError("ReleaseDate", "You have to enter a ReleaseDate!");
                return View();
            }
            if (upcomingUpdateVM.Point == null)
            {
                ModelState.AddModelError("Point", "You have to enter a Point!");
                return View();
            }
            if (upcomingUpdateVM.TypeId == 0)
            {
                ModelState.AddModelError("TypeId", "You have to enter a TypeId!");
                return View();
            }

            if (upcomingUpdateVM.Photo != null)
            {

                if (!upcomingUpdateVM.Photo.ContentType.Contains("image"))
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }


                if (!upcomingUpdateVM.Photo.IsImage())
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/img/poster", updatedUpcoming.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                updatedUpcoming.ImageUrl = upcomingUpdateVM.Photo.SaveImage(_env, "assets/img/poster", upcomingUpdateVM.Photo.FileName);
                updatedUpcoming.Name = upcomingUpdateVM.Name;
                updatedUpcoming.Point = upcomingUpdateVM.Point;
                updatedUpcoming.Duration = upcomingUpdateVM.Duration;
                updatedUpcoming.ReleaseDate = upcomingUpdateVM.ReleaseDate;
                updatedUpcoming.Quality = upcomingUpdateVM.Quality;
                updatedUpcoming.ReleaseDate = upcomingUpdateVM.ReleaseDate;
                updatedUpcoming.TypeId = upcomingUpdateVM.TypeId;



                _appDbContext.SaveChanges();


                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Photo", "You have to enter a Photo!");
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Upcoming deletedUpcoming = _appDbContext.Upcomings.Find(id);

            if (deletedUpcoming == null) return NotFound();

            _appDbContext.Remove(deletedUpcoming);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

