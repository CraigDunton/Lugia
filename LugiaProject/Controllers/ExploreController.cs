using HtmlAgilityPack;
using LugiaProject.Data;
using LugiaProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            
            var interests = _dbContext.Interests.Where(u => u.UserId == user.Id).ToList();
            int r = rand.Next(interests.Count);
            var interest = interests.ElementAt(r);

            ExploreModel eModel = new ExploreModel()
            {
                Query = interest.Name

            };
            var scr = new BingScraper();
            eModel.Result = scr.ParseBingUrls(eModel.Query);
            //var scr = new DuckScraper();
            //eModel.Result = scr.ParseDuckUrls(eModel.Query);
            Console.WriteLine("Done");
            return View("Index", eModel);
        }

      
        [HttpPost]
        public async Task<IActionResult> Index(ExploreModel eModel)
        {
            
            //if (TempData["Query"] != null)
            //{
            //    eModel.Query = (String)ViewData["Query"];
            //    eModel.Result = (List<StumbleModel>)ViewData["Result"];
            //}

            ApplicationUser user = await _userManager.GetUserAsync(User);

            

            if (eModel == null || eModel.Result.Count == 0)
            {
                Random rand = new Random();
                
                var interests = _dbContext.Interests.Where(u => u.UserId == user.Id).ToList();
                int r = rand.Next(interests.Count);
                var interest = interests.ElementAt(r);
                eModel = new ExploreModel()
                {
                    Query = interest.Name

                };
                var scr = new BingScraper();
                eModel.Result = scr.ParseBingUrls(eModel.Query);
                //var scr = new DuckScraper();
                //eModel.Result = scr.ParseDuckUrls(eModel.Query);
                Console.WriteLine("Done");
                return View("Index", eModel);
            }
            else
            {
                eModel.Result.Remove(eModel.Result.First());
                ModelState.Clear();
                return View("Index", eModel);
            }

        }

        [HttpPost]
        public IActionResult Explore(ExploreModel model)
        {

            if (model.Result.Count == 0)
            {
                var scr = new BingScraper();
                model.Result = scr.ParseBingUrls(model.Query);
                //var scr = new DuckScraper();
                //model.Result = scr.ParseDuckUrls(model.Query);
                Console.WriteLine("Done");
                return View("Index", model);
            } else
            {
                //model.Result.Remove(model.Result.First());
                return View("Index", model);
            }
        }
    }

    public class BingScraper
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

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(n.First().Attributes["href"].Value);
                request.UseDefaultCredentials = true;
                try
                {
                    //don't know why to use using but the internet says so
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        String header = response.GetResponseHeader("X-Frame-Options");
                        Console.Write("Here is header: " + header + "\n");

                        if (!(header.ToLower().Equals("deny")|| header.ToLower().Equals("sameorigin"))) {

                            var stm = new StumbleModel()
                            {

                                Title = n.First().InnerText,
                                Url = n.First().Attributes["href"].Value
                            };

                            sites.Add(stm);

                        }
                    }
                }
                //in case of 400, 302, idk shit hits the fan sometimes
                catch (WebException e)
                {
                    if (!e.Status.Equals(WebExceptionStatus.UnknownError))
                    {
                        using (WebResponse response = e.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            Console.WriteLine("Error code: {0} ", httpResponse.StatusCode);
                        }
                    }
                }

            }


            return sites;
        }
    }

    public class DuckScraper
    {

        private string se = "https://duckduckgo.com/?q=";

        public List<StumbleModel> ParseDuckUrls(string query)
        {
            var sites = new List<StumbleModel>();
            var web = new HtmlWeb();
            var doc = web.Load(se + query);

            //.Select(t => t.Descendants("a"))

            var nodes = doc.DocumentNode.Descendants("div")
                           .Select(t => t.Descendants("a")
                           .Where(d => d.Attributes.Contains("class")
                            && d.Attributes["class"].Value.Contains("result__title")))
                           .ToList();

            //TODO: fix document is too complex to parse
            foreach (var n in nodes)
            {
                //var w = new HtmlWeb();
                //var d = web.Load(n.First().Attributes["href"].Value);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(n.First().Attributes["href"].Value);
                request.UseDefaultCredentials = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                String header = response.GetResponseHeader("X-Frame-Options");
                Console.Write("Here is header: " + header + "\n");

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