using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> _logger;
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramController(ILogger<TelegramController> logger, ITelegramBotClient telegramBotClient)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
        }

        [HttpPost]
        [Route("api/telegram")]
        public IActionResult Post([FromBody] JObject requestData)
        {
            if (requestData == null)
            {
                _logger.LogWarning("Received null request data");
                return BadRequest();
            }

            if (requestData["message"] != null)
            {
                var messageText = requestData["message"]["text"].ToString();
                _logger.LogInformation($"Received message: {messageText}");
                _telegramBotClient.SendTextMessageAsync(
                    chatId: requestData["message"]["chat"]["id"].ToString(),
                    text: $"You said: {messageText}");
            }

            return Ok();
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Telegram webhook is working.");
        }
    }
}
