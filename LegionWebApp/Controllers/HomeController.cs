using LegionWebApp.Data;
using LegionWebApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
		public IActionResult Gallery()
		{
			var galleryModel = new GalleryModel(_dbContext);
			return View(galleryModel.ItemList);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}