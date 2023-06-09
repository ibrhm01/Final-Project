using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class PricingController : Controller
    {
        public readonly AppDbContext _appDbContext;

        public PricingController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Pricing> pricings = _appDbContext.Pricings
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Pricings.ToList(), take);
            PaginationVM<Pricing> pagination = new(pricings, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Pricing> pricings, int take)
        {
            return (int)Math.Ceiling((double)pricings.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            Pricing pricing = _appDbContext.Pricings.Where(m => m.Id == id).FirstOrDefault();
            return View(pricing);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PricingCreateVM pricingCreateVM)
        {

            if (pricingCreateVM.PricingType == null)
            {
                ModelState.AddModelError("PricingType", "You have to enter a PricingType!");
                return View();
            }
            if (pricingCreateVM.Price == null)
            {
                ModelState.AddModelError("Price", "You have to enter a Price!");
                return View();
            }
            if (pricingCreateVM.Resolution == null)
            {
                ModelState.AddModelError("Resolution", "You have to enter a Resolution!");
                return View();
            }
            if (pricingCreateVM.VideoQuality == null)
            {
                ModelState.AddModelError("VideoQuality", "You have to enter a VideoQuality!");
                return View();
            }
            if (pricingCreateVM.AmountOfScreens == null)
            {
                ModelState.AddModelError("AmountOfScreens", "You have to enter a AmountOfScreens!");
                return View();
            }
            if (pricingCreateVM.Cancel == null)
            {
                ModelState.AddModelError("Cancel", "You have to enter an Cancel!");
                return View();
            }



            Pricing newPricing = new();
            newPricing.PricingType = pricingCreateVM.PricingType;
            newPricing.Price = pricingCreateVM.Price;
            newPricing.Resolution = pricingCreateVM.Resolution;
            newPricing.VideoQuality = pricingCreateVM.VideoQuality;
            newPricing.AmountOfScreens = pricingCreateVM.AmountOfScreens;
            newPricing.Cancel = pricingCreateVM.Cancel;


            _appDbContext.Pricings.Add(newPricing);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            Pricing updatedPricing = _appDbContext.Pricings.Find(id);

            if (updatedPricing == null) return NotFound();

            return View(new PricingUpdateVM
            {
                PricingType = updatedPricing.PricingType,
                Price = updatedPricing.Price,
                Resolution = updatedPricing.Resolution,
                VideoQuality = updatedPricing.VideoQuality,
                AmountOfScreens = updatedPricing.AmountOfScreens,
                Cancel = updatedPricing.Cancel,

            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, PricingUpdateVM pricingUpdateVM)
        {
            if (id == null) return NotFound();

            Pricing updatedPricing = _appDbContext.Pricings.Find(id);

            if (updatedPricing == null) return NotFound();


            if (pricingUpdateVM.PricingType == null)
            {
                ModelState.AddModelError("PricingType", "You have to enter a PricingType!");
                return View();
            }
            if (pricingUpdateVM.Price == null)
            {
                ModelState.AddModelError("Price", "You have to enter a Price!");
                return View();
            }
            if (pricingUpdateVM.Resolution == null)
            {
                ModelState.AddModelError("Resolution", "You have to enter a Resolution!");
                return View();
            }
            if (pricingUpdateVM.VideoQuality == null)
            {
                ModelState.AddModelError("VideoQuality", "You have to enter a VideoQuality!");
                return View();
            }
            if (pricingUpdateVM.AmountOfScreens == null)
            {
                ModelState.AddModelError("AmountOfScreens", "You have to enter a AmountOfScreens!");
                return View();
            }
            if (pricingUpdateVM.Cancel == null)
            {
                ModelState.AddModelError("Cancel", "You have to enter an Cancel!");
                return View();
            }

            updatedPricing.PricingType = pricingUpdateVM.PricingType;
            updatedPricing.Price = pricingUpdateVM.Price;
            updatedPricing.Resolution = pricingUpdateVM.Resolution;
            updatedPricing.VideoQuality = pricingUpdateVM.VideoQuality;
            updatedPricing.AmountOfScreens = pricingUpdateVM.AmountOfScreens;
            updatedPricing.Cancel = pricingUpdateVM.Cancel;

            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Pricing deletedPricing = _appDbContext.Pricings.Find(id);

            if (deletedPricing == null) return NotFound();

            _appDbContext.Remove(deletedPricing);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

