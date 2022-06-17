using LegionWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Diagnostics;

namespace LegionWebApp.Controllers
{
    
    public class ENController : Controller
    {
        private readonly ILogger<ENController> _logger;
        public ENController(ILogger<ENController> logger)
        {
            _logger = logger;
        }

        public IActionResult Vehicles()
        {
            return View();
        }

        public IActionResult Needs()
        {
            return View();
        }

        public IActionResult Finance()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Index()
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