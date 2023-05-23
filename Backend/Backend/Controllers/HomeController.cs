using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.DAL;
using Backend.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _appDbContext;

    public HomeController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Index()
    {
        HomeVM homeVM = new();
        homeVM.Banner = _appDbContext.Banners.Include(b => b.Categories).FirstOrDefault();
        homeVM.Types = _appDbContext.Types.Where(t => t.IsDeleted != true).ToList();
        homeVM.UpcomingMovies = _appDbContext.UpcomingMovies.Where(t => t.IsDeleted != true).ToList();
        homeVM.Service = _appDbContext.Services.Where(s => s.IsDeleted != true).FirstOrDefault();
        homeVM.BestSeries = _appDbContext.BestSeries.Where(s => s.IsDeleted != true).ToList();
        homeVM.TrialTest = _appDbContext.TrialTests.Where(s => s.IsDeleted != true).FirstOrDefault();




        return View(homeVM);
    }
}

