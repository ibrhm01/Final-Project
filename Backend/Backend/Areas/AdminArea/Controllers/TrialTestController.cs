using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more TrialTest on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class TrialTestController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public TrialTestController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            TrialTest trialTest = _appDbContext.TrialTests.Where(i => i.IsDeleted != true).FirstOrDefault();

            return View(trialTest);
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            TrialTest updatedTrialTest = _appDbContext.TrialTests.Find(id);

            if (updatedTrialTest == null) return NotFound();

            return View(new TrialTestUpdateVM
            {
                BackgroundImageUrl = updatedTrialTest.BackgroundImageUrl,
                Title = updatedTrialTest.Title,
                Description = updatedTrialTest.Description,
                Placeholder = updatedTrialTest.Placeholder,
                
            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, TrialTestUpdateVM TrialTestUpdateVM)
        {
            if (id == null) return NotFound();

            TrialTest updatedTrialTest = _appDbContext.TrialTests.Find(id);

            if (updatedTrialTest == null) return NotFound();


            if (TrialTestUpdateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter Title!");
                return View();
            }
            if (TrialTestUpdateVM.Description == null)
            {
                ModelState.AddModelError("Description", "You have to enter a Description!");
                return View();
            }
            if (TrialTestUpdateVM.Placeholder == null)
            {
                ModelState.AddModelError("Placeholder", "You have to enter Placeholder!");
                return View();
            }
           

            if (TrialTestUpdateVM.Photo != null)
            {

                if (!TrialTestUpdateVM.Photo.ContentType.Contains("image"))
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }


                if (!TrialTestUpdateVM.Photo.IsImage())
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/img/bg", updatedTrialTest.BackgroundImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                updatedTrialTest.BackgroundImageUrl = TrialTestUpdateVM.Photo.SaveImage(_env, "assets/img/bg", TrialTestUpdateVM.Photo.FileName);
                updatedTrialTest.Title = TrialTestUpdateVM.Title;
                updatedTrialTest.Description = TrialTestUpdateVM.Description;
                updatedTrialTest.Placeholder = TrialTestUpdateVM.Placeholder;
                _appDbContext.SaveChanges();


                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Photo", "You have to enter a Photo!");
            return View();
        }
    }
}

