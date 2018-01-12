using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LugiaProject.Models;
using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Linq;

namespace LugiaProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Explore()
        {
            ViewData["Message"] = "Your explore page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Scrape()
        {
            return null;
        }
    }

}
