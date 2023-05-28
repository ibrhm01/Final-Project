using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.DAL;
using Backend.ViewModels;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;

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
        homeVM.Upcomings = _appDbContext.Upcomings.Where(t => t.IsDeleted != true).ToList();
        homeVM.Service = _appDbContext.Services.Where(s => s.IsDeleted != true).FirstOrDefault();
        homeVM.BestSeries = _appDbContext.BestSeries.Where(s => s.IsDeleted != true).ToList();
        homeVM.TrialTest = _appDbContext.TrialTests.Where(s => s.IsDeleted != true).FirstOrDefault();
        homeVM.Streaming = _appDbContext.Streamings.Where(s => s.IsDeleted != true).FirstOrDefault();
        homeVM.TopRateds = _appDbContext.TopRateds.Include(t=>t.TopRatedCategories).ThenInclude(tc=>tc.Category).ToList();
        homeVM.Categories = _appDbContext.Categories.ToList();


        return View(homeVM);
    }
}

 