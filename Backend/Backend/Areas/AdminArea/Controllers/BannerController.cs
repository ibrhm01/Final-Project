using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class BannerController : Controller
    {
        // GET: /<controller>/
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public BannerController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            Banner Banner = _appDbContext.Banners.Where(i => i.IsDeleted != true).FirstOrDefault();

            return View(Banner);
        }
        public IActionResult Detail(int id)
        {
            Banner banner = _appDbContext.Banners.Where(m => m.Id == id).FirstOrDefault();
            return View(banner);
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            Banner updatedBanner = _appDbContext.Banners.Find(id);

            if (updatedBanner == null) return NotFound();

            return View(new BannerUpdateVM
            {
                ImageUrl = updatedBanner.ImageUrl,
                Title = updatedBanner.Title,
                Description = updatedBanner.Description,
                BrandName = updatedBanner.BrandName,
                Quality = updatedBanner.Quality,
                Duration = updatedBanner.Duration,
                ReleaseDate = updatedBanner.ReleaseDate,
                TeaserUrl = updatedBanner.TeaserUrl,
                Status = updatedBanner.Status,

            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, BannerUpdateVM bannerUpdateVM)
        {
            if (id == null) return NotFound();

            Banner updatedBanner = _appDbContext.Banners.Find(id);

            if (updatedBanner == null) return NotFound();


            if (bannerUpdateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter Title!");
                return View();
            }
            if (bannerUpdateVM.Description == null)
            {
                ModelState.AddModelError("Description", "You have to enter a Description!");
                return View();
            }
            if (bannerUpdateVM.BrandName == null)
            {
                ModelState.AddModelError("BrandName", "You have to enter BrandName!");
                return View();
            }
            if (bannerUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter Quality!");
                return View();
            }
            if (bannerUpdateVM.Duration == null)
            {
                ModelState.AddModelError("Duration", "You have to enter Duration!");
                return View();
            }
            if (bannerUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter an Quality!");
                return View();
            }
            if (bannerUpdateVM.TeaserUrl == null)
            {
                ModelState.AddModelError("TeaserUrl", "You have to enter TeaserUrl!");
                return View();
            }
            if (bannerUpdateVM.Status == null)
            {
                ModelState.AddModelError("Status", "You have to enter Status!");
                return View();
            }

            if (bannerUpdateVM.Photo != null)
            {

                if (!bannerUpdateVM.Photo.ContentType.Contains("image"))
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }


                if (!bannerUpdateVM.Photo.IsImage())
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/img/banner", updatedBanner.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                updatedBanner.ImageUrl = bannerUpdateVM.Photo.SaveImage(_env, "assets/img/banner", bannerUpdateVM.Photo.FileName);
                updatedBanner.Title = bannerUpdateVM.Title;
                updatedBanner.Description = bannerUpdateVM.Description;
                updatedBanner.BrandName = bannerUpdateVM.BrandName;
                updatedBanner.Quality = bannerUpdateVM.Quality;
                updatedBanner.Duration = bannerUpdateVM.Duration;
                updatedBanner.ReleaseDate = bannerUpdateVM.ReleaseDate;
                updatedBanner.TeaserUrl = bannerUpdateVM.TeaserUrl;
                updatedBanner.Status = bannerUpdateVM.Status;
                
                _appDbContext.SaveChanges();


                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Photo", "You have to enter a Photo!");
            return View();
        }
    }
}

