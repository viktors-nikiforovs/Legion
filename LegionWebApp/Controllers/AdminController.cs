using Microsoft.AspNetCore.Mvc;

namespace LegionWebApp.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
