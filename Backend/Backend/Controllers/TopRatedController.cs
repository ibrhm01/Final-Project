using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class TopRatedController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TopRatedController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        

        public IActionResult Detail(int id)
        {
            TopRatedDetailVM topRatedDetailVM = new();
            topRatedDetailVM.TopRated = _appDbContext.TopRateds.Where(m => m.Id == id).Include(m => m.TopRatedCategories).ThenInclude(mc => mc.Category).FirstOrDefault();
            topRatedDetailVM.TopRateds = _appDbContext.TopRateds.Where(m => m.Id != id).Include(m => m.TopRatedCategories).ThenInclude(mc => mc.Category).ToList();


            topRatedDetailVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();

            return View(topRatedDetailVM);
        }
    }
}

