using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more Streaming on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class StreamingController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public StreamingController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            Streaming streaming = _appDbContext.Streamings.Where(i => i.IsDeleted != true).FirstOrDefault();

            return View(streaming);
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            Streaming updatedStreaming = _appDbContext.Streamings.Find(id);

            if (updatedStreaming == null) return NotFound();

            return View(new StreamingUpdateVM
            {
                Title = updatedStreaming.Title,
                Description = updatedStreaming.Description,
                ActiveCustomer = updatedStreaming.ActiveCustomer,
                Quality = updatedStreaming.Quality,

            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, StreamingUpdateVM streamingUpdateVM)
        {
            if (id == null) return NotFound();

            Streaming updatedStreaming = _appDbContext.Streamings.Find(id);

            if (updatedStreaming == null) return NotFound();


            if (streamingUpdateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter Title!");
                return View();
            }
            if (streamingUpdateVM.Description == null)
            {
                ModelState.AddModelError("Description", "You have to enter a Description!");
                return View();
            }
            if (streamingUpdateVM.ActiveCustomer == null)
            {
                ModelState.AddModelError("ActiveCustomer", "You have to enter ActiveCustomer!");
                return View();
            }
            if (streamingUpdateVM.Quality == null)
            {
                ModelState.AddModelError("Quality", "You have to enter an Quality!");
                return View();
            }

            if (streamingUpdateVM.Photo != null)
            {

                if (!streamingUpdateVM.Photo.ContentType.Contains("image"))
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }


                if (!streamingUpdateVM.Photo.IsImage())
                {

                    ModelState.AddModelError("Photo", "You have to upload an image!");
                    return View();

                }

                string fullPath = Path.Combine(_env.WebRootPath, "assets/img/images", updatedStreaming.ImageUrl);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }


                updatedStreaming.ImageUrl = streamingUpdateVM.Photo.SaveImage(_env, "assets/img/images", streamingUpdateVM.Photo.FileName);
                updatedStreaming.Title = streamingUpdateVM.Title;
                updatedStreaming.Description = streamingUpdateVM.Description;
                updatedStreaming.ActiveCustomer = streamingUpdateVM.ActiveCustomer;
                updatedStreaming.Quality = streamingUpdateVM.Quality;

                _appDbContext.SaveChanges();


                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Photo", "You have to enter a Photo!");
            return View();
        }
    }
}

