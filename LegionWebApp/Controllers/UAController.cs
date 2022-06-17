﻿using LegionWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Diagnostics;

namespace LegionWebApp.Controllers
{
    
    public class UAController : Controller
    {
        private readonly ILogger<UAController> _logger;
        public UAController(ILogger<UAController> logger)
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}