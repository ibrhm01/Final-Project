using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class TvSeriesController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TvSeriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            TvSeriesVM tvSeriesVM = new();

            tvSeriesVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();
            tvSeriesVM.Categories = _appDbContext.Categories.ToList();
            tvSeriesVM.TvSeries = _appDbContext.TvSeries.Include(t => t.TvSeriesCategories).ThenInclude(tc => tc.Category).ToList();


            return View(tvSeriesVM);
        }

        public IActionResult Detail(int id)
        {
            TvSeriesDetailVM tvSeriesDetailVM = new();
            tvSeriesDetailVM.TvSeries = _appDbContext.TvSeries
                .Where(ts=>ts.Id==id)
                .Include(t=>t.Seasons)
                .ThenInclude(s=>s.Episodes)
                .Include(t => t.TvSeriesCategories)
                .ThenInclude(tc => tc.Category)
                .FirstOrDefault();
            tvSeriesDetailVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();
            tvSeriesDetailVM.Categories = _appDbContext.Categories.ToList();
            tvSeriesDetailVM.TvSeriesList = _appDbContext.TvSeries.Include(t => t.TvSeriesCategories).ThenInclude(tc => tc.Category).ToList();
            return View(tvSeriesDetailVM);
        }

    }
}

