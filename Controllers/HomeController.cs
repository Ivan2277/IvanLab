using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarsLab.Models;

namespace CarsLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ModelCars()
        {
            return RedirectToAction("Index", "ModelCars");
        }
        public IActionResult BodyTypes()
        {
            return RedirectToAction("Index", "BodyTypes");
        }
        public IActionResult Engines()
        {
            return RedirectToAction("Index", "Engines");
        }
        public IActionResult PriceCategories()
        {
            return RedirectToAction("Index", "PriceCategories");
        }
        public IActionResult YearOfIssues()
        {
            return RedirectToAction("Index", "YearOfIssues");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
