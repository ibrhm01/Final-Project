using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class InformationController : Controller
    {
        public readonly AppDbContext _appDbContext;

        public InformationController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            Information information = _appDbContext.Informations.Where(i => i.IsDeleted != true).FirstOrDefault();

            return View(information);
        }

        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();

            Information updatedInformation = _appDbContext.Informations.Find(id);

            if (updatedInformation == null) return NotFound();

            return View(new InformationUpdateVM
            {
                About = updatedInformation.About,
                Address = updatedInformation.Address,
                Phone = updatedInformation.Phone,
                Email = updatedInformation.Email,
                
            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, InformationUpdateVM InformationUpdateVM)
        {
            if (id == null) return NotFound();

            Information updatedInformation = _appDbContext.Informations.Find(id);

            if (updatedInformation == null) return NotFound();


            if (InformationUpdateVM.About == null)
            {
                ModelState.AddModelError("About", "You have to enter About!");
                return View();
            }
            if (InformationUpdateVM.Address == null)
            {
                ModelState.AddModelError("Address", "You have to enter an Address!");
                return View();
            }
            if (InformationUpdateVM.Phone == null)
            {
                ModelState.AddModelError("Phone", "You have to enter a Phone!");
                return View();
            }
            if (InformationUpdateVM.Email == null)
            {
                ModelState.AddModelError("Email", "You have to enter an Email!");
                return View();
            }
            

            updatedInformation.About = InformationUpdateVM.About;
            updatedInformation.Address = InformationUpdateVM.Address;
            updatedInformation.Phone = InformationUpdateVM.Phone;
            updatedInformation.Email = InformationUpdateVM.Email;
           
            _appDbContext.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}

