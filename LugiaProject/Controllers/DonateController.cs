using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LugiaProject.Data;
using LugiaProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LugiaProject.Controllers
{
    public class DonateController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonateController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //get all nonprofits to show in a table
            DonateViewModel model = new DonateViewModel();

            List<NonProfitModel> nonProfits = _dbContext.NonProfits.ToList();
            ApplicationUser user = await _userManager.GetUserAsync(User);

            model.User = user;
            model.NonProfitsList = nonProfits;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DonateViewModel model)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            //add points from this model to the nonprofit and deduct from user
            foreach (NonProfitModel nonProfit in model.NonProfitsList)
            {
                //TODO: Error message for invalid point allocatoin
                if (nonProfit.PointsFromUser > 0 && user.Points>=nonProfit.PointsFromUser)
                {
                    var nonProfitDb = _dbContext.NonProfits.First(m => m.Id == nonProfit.Id);
                    nonProfitDb.Points += nonProfit.PointsFromUser;
                    user.Points -= nonProfit.PointsFromUser;
                    //set this back to zero once transaction is complete
                    nonProfit.PointsFromUser = 0;
                    nonProfitDb.PointsFromUser = 0;
                }
            }
            if (ModelState.IsValid)
            {
                _dbContext.SaveChanges();
            }
            model.User = user;
            model.NonProfitsList = _dbContext.NonProfits.ToList();

            return RedirectToAction("Index", "Donate");
        }
    }
}