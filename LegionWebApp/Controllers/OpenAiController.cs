using LegionWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LegionWebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OpenAiController : Controller
	{
		private readonly ILogger<OpenAiController> _logger;
		private readonly IOpenAiService _openAiService;

		public OpenAiController(ILogger<OpenAiController> logger, IOpenAiService openAiService)
		{
			_logger = logger;
			_openAiService = openAiService;
		}


		[HttpPost()]
		[Route("CompleteSentence")]
		public async Task<IActionResult> CompleteSentence(string text)
		{
			var result = await _openAiService.CompleteSentence(text);
			return Ok(result);
		}

		[HttpGet]
		[Route("Translate")]
		public async Task<IActionResult> Translate(string lang, string text)
		{
			var result = await _openAiService.TranslateText(lang, text);
			return Ok(result);
		}
	}
}
