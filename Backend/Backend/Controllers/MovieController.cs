using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public MovieController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            MovieVM movieVM = new();
            movieVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();
            movieVM.Categories = _appDbContext.Categories.ToList();
            movieVM.Movies = _appDbContext.Movies
                .Include(t => t.MovieCategories)
                .ThenInclude(tc => tc.Category)
                .ToList();

            return View(movieVM);
        }

    }
}

