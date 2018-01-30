using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LugiaProject.Models;
using HtmlAgilityPack;
using LugiaProject.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LugiaProject.Controllers
{
    public class ExploreController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ExploreController(
            ApplicationDbContext dbContext,
          UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Random rand = new Random();

            //TODO: get only one of a users interests. Not sure the model is set up
            var interests = _dbContext.Interests.Where(u => u.UserId == user.Id).ToList();
            int r = rand.Next(interests.Count);
            var interest = interests.ElementAt(r);

            ExploreModel eModel = new ExploreModel()
            {
                Query = interest.Name

            };
            var scr = new Scraper();
            eModel.Result = scr.ParseBingUrls(eModel.Query);

            return View(eModel);
        }
        [HttpPost]
        public IActionResult Explore(ExploreModel model)
        {
            var scr = new Scraper();
            model.Result = scr.ParseBingUrls(model.Query);
            Console.WriteLine("Done");
            return View("Index", model);
        }
    }

    public class Scraper
    {
        private string se = "https://www.bing.com/search?q=";

        public List<StumbleModel> ParseBingUrls(string query)
        {
            var sites = new List<StumbleModel>();
            var web = new HtmlWeb();
            var doc = web.Load(se + query);

            var nodes = doc.DocumentNode.Descendants("li")
                           .Where(d => d.Attributes.Contains("class")
                            && d.Attributes["class"].Value.Contains("b_algo"))
                           .Select(t => t.Descendants("a"))
                           .ToList();
            foreach (var n in nodes)
            {
                //var w = new HtmlWeb();
                //var d = web.Load(n.First().Attributes["href"].Value);
                var stm = new StumbleModel()
                {
                    Title = n.First().InnerText,
                    Url = n.First().Attributes["href"].Value
                };
                sites.Add(stm);
            }


            return sites;
        }
    }
}