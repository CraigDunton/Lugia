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
        public IActionResult Index(DonateViewModel model)
        {
            //add points from this model to the DB

            return View();
        }
    }
}