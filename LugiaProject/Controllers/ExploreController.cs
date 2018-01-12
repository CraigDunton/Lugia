using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LugiaProject.Models;
using HtmlAgilityPack;

namespace LugiaProject.Controllers
{
    public class ExploreController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
                var w = new HtmlWeb();
                var d = web.Load(n.First().Attributes["href"].Value);
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