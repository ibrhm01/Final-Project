using System;
using Backend.DAL;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.ViewComponents
{
	public class FooterViewComponent:ViewComponent
	{
        private readonly AppDbContext _appDbContext;

        public FooterViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bio bio = _appDbContext.Bios.Where(m => m.IsDeleted != true).FirstOrDefault();

            return View(await Task.FromResult(bio));
        }
    }
}

