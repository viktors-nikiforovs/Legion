using LegionWebApp.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LegionWebApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
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
            Console.WriteLine("Index page loaded");
            return View();
        }
        public async Task<IActionResult> Gallery()
        {
            TelegramBot bot = new TelegramBot();
            await bot.SendMessageAsync("Hello World!");
            return View();
        }

        [HttpPost]
        public async Task Post([FromBody] Update update)
        {
            _logger.LogInformation("Post action executed");

            TelegramBot bot = new TelegramBot();
            await bot.SendMessageAsync("Hello World!");

            TelegramBotClient client = new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN"));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                _logger.LogInformation($"Sending message to {update.Message.From.Id}");
                await client.SendTextMessageAsync(update.Message.From.Id, "answer");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}