using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class ServiceController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public ServiceController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            Service service = _appDbContext.Services.Where(i => i.IsDeleted != true).FirstOrDefault();

            return View(service);
        }

        public IActionResult Detail(int id)
        {
            Service service = _appDbContext.Services.Where(m => m.Id == id).FirstOrDefault();
            return View(service);
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            Service updatedService = _appDbContext.Services.Find(id);

            if (updatedService == null) return NotFound();

            return View(new ServiceUpdateVM
            {
                ImageUrl = updatedService.ImageUrl,
                Description = updatedService.Description,
                Title= updatedService.Title,
                SubDescription1 = updatedService.SubDescription1,
                SubDescription2 = updatedService.SubDescription2,
                SubTitle1 = updatedService.SubTitle1,
                SubTitle2 = updatedService.SubTitle2,
            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, ServiceUpdateVM serviceUpdateVM)
        {
            if (id == null) return NotFound();

            Service updatedService = _appDbContext.Services.Find(id);

            if (updatedService == null) return NotFound();


            if (serviceUpdateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter Title!");
                return View();
            }
            if (serviceUpdateVM.Description == null)
            {
                ModelState.AddModelError("Description", "You have to enter a Description!");
                return View();
            }
            if (serviceUpdateVM.SubDescription1 == null)
            {
                ModelState.AddModelError("SubDescription1", "You have to enter SubDescription1!");
                return View();
            }
            if (serviceUpdateVM.SubDescription2 == null)
            {
                ModelState.AddModelError("SubDescription2", "You have to enter an SubDescription2!");
                return View();
            }
            if (serviceUpdateVM.SubTitle1 == null)
            {
                ModelState.AddModelError("SubTitle1", "You have to enter an SubTitle1!");
                return View();
            }
            if (serviceUpdateVM.SubTitle2 == null)
            {
                ModelState.AddModelError("SubTitle2", "You have to enter an SubTitle2!");
                return View();
            }

            if (serviceUpdateVM.Photo != null)
            {

                if (!serviceUpdateVM.Photo.ContentType.Contains("image"))
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }


                if (!serviceUpdateVM.Photo.IsImage())
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/img/images", updatedService.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                updatedService.ImageUrl = serviceUpdateVM.Photo.SaveImage(_env, "assets/img/images", serviceUpdateVM.Photo.FileName);
                updatedService.Title = serviceUpdateVM.Title;
                updatedService.Description = serviceUpdateVM.Description;
                updatedService.SubTitle1 = serviceUpdateVM.SubTitle1;
                updatedService.SubTitle2 = serviceUpdateVM.SubTitle2;
                updatedService.SubDescription1 = serviceUpdateVM.SubDescription1;
                updatedService.SubDescription2 = serviceUpdateVM.SubDescription2;

                _appDbContext.SaveChanges();


                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Photo", "You have to enter a Photo!");
            return View();
        }
    }
}

