using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
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

    public class SeasonController : Controller
    {
        public readonly AppDbContext _appDbContext;

        public SeasonController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Season> seasons = _appDbContext.Seasons
                .Skip((page - 1) * take)
                .Take(take)
                .Include(s=>s.TvSeries)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Seasons.ToList(), take);
            PaginationVM<Season> pagination = new(seasons, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Season> seasons, int take)
        {
            return (int)Math.Ceiling((double)seasons.Count / (double)take);
        }

        
        public IActionResult Create()
        {
            ViewBag.TvSeries = new SelectList(_appDbContext.TvSeries.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(SeasonCreateVM seasonCreateVM)
        {

            if (seasonCreateVM.SeasonNumber == null)
            {
                ModelState.AddModelError("SeasonNumber", "You have to enter a SeasonNumber!");
                return View();
            }
            
            
            Season newSeason = new();
            newSeason.SeasonNumber = seasonCreateVM.SeasonNumber;
            newSeason.TvSeriesId = seasonCreateVM.TvSeriesId;
            


            _appDbContext.Seasons.Add(newSeason);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            ViewBag.TvSeries = new SelectList(_appDbContext.TvSeries.ToList(), "Id", "Name");


            Season updatedSeason = _appDbContext.Seasons.Find(id);

            if (updatedSeason == null) return NotFound();

            return View(new SeasonUpdateVM
            {
                SeasonNumber = updatedSeason.SeasonNumber,
                TvSeriesId = updatedSeason.TvSeriesId
              
            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, SeasonUpdateVM seasonUpdateVM)
        {
            if (id == null) return NotFound();

            Season updatedSeason = _appDbContext.Seasons.Find(id);

            if (updatedSeason == null) return NotFound();


            if (seasonUpdateVM.SeasonNumber == null)
            {
                ModelState.AddModelError("PricingType", "You have to enter a PricingType!");
                return View();
            }


            updatedSeason.SeasonNumber = seasonUpdateVM.SeasonNumber;
            updatedSeason.TvSeriesId = seasonUpdateVM.TvSeriesId;
            

            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }

        
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Season deletedSeason = _appDbContext.Seasons.Find(id);

            if (deletedSeason == null) return NotFound();

            _appDbContext.Remove(deletedSeason);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

