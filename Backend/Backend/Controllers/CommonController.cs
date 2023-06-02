using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class CommonController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CommonController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Search(string search)
        {
            SearchPartialVM searchPartialVM = new();

            if (!String.IsNullOrEmpty(search))
            {
                searchPartialVM.Movies = _appDbContext.Movies
                    .Where(m => m.Name.ToLower()
                    .Contains(search.ToLower()))
                    .Take(4)
                    .OrderByDescending(p => p.Id)
                    .ToList();
                searchPartialVM.TvSeries = _appDbContext.TvSeries
                   .Where(m => m.Name.ToLower()
                   .Contains(search.ToLower()))
                   .Take(4)
                   .OrderByDescending(p => p.Id)
                   .ToList();
            }
            return PartialView("_SearchPartial", searchPartialVM);
        }
    }
}

