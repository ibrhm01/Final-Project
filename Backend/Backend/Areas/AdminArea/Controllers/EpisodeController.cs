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

    public class EpisodeController : Controller
    {
        public readonly AppDbContext _appDbContext;

        public EpisodeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Episode> episodes = _appDbContext.Episodes
                .Skip((page - 1) * take)
                .Take(take)
                .Include(e=>e.Season)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Episodes.ToList(), take);
            PaginationVM<Episode> pagination = new(episodes, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Episode> episodes, int take)
        {
            return (int)Math.Ceiling((double)episodes.Count / (double)take);
        }


        public IActionResult Create()
        {
            ViewBag.Seasons = new SelectList(_appDbContext.Seasons.ToList(), "Id", "SeasonNumber");
            return View();
        }

        [HttpPost]
        public IActionResult Create(EpisodeCreateVM episodeCreateVM)
        {

            if (episodeCreateVM.EpisodeNumber == null)
            {
                ModelState.AddModelError("EpisodeNumber", "You have to enter a EpisodeNumber!");
                return View();
            }


            Episode newEpisode = new();
            newEpisode.Name = episodeCreateVM.Name;
            newEpisode.EpisodeNumber = episodeCreateVM.EpisodeNumber;
            newEpisode.SeasonId = episodeCreateVM.SeasonId;



            _appDbContext.Episodes.Add(newEpisode);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            ViewBag.Seasons = new SelectList(_appDbContext.Seasons.ToList(), "Id", "SeasonNumber");


            Episode updatedEpisode = _appDbContext.Episodes.Find(id);

            if (updatedEpisode == null) return NotFound();

            return View(new EpisodeUpdateVM
            {
                Name = updatedEpisode.Name,
                EpisodeNumber = updatedEpisode.EpisodeNumber,
                SeasonId = updatedEpisode.SeasonId

            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, EpisodeUpdateVM episodeUpdateVM)
        {
            if (id == null) return NotFound();

            Episode updatedEpisode = _appDbContext.Episodes.Find(id);

            if (updatedEpisode == null) return NotFound();


            if (episodeUpdateVM.EpisodeNumber == null)
            {
                ModelState.AddModelError("PricingType", "You have to enter a PricingType!");
                return View();
            }

            updatedEpisode.Name = episodeUpdateVM.Name;
            updatedEpisode.EpisodeNumber = episodeUpdateVM.EpisodeNumber;
            updatedEpisode.SeasonId = episodeUpdateVM.SeasonId;


            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Episode deletedEpisode = _appDbContext.Episodes.Find(id);

            if (deletedEpisode == null) return NotFound();

            _appDbContext.Remove(deletedEpisode);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

