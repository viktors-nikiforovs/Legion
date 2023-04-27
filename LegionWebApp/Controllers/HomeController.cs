using LegionWebApp.Data;
using LegionWebApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace LegionWebApp.Controllers
{

	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _dbContext;
		public HomeController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpPost]
		public IActionResult CultureManagement(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
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
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Gallery(int page = 1, int pageSize = 1)
		{
			var galleryModel = new GalleryModel(_dbContext);
			var pagedItems = galleryModel.ItemList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{				
				return PartialView("_GalleryItems", pagedItems.FirstOrDefault());				
			}
			
			return View(pagedItems);
		}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}