using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LegionWebApp.Controllers
{
    [Route("api/telegram")]
    public class TelegramController : Controller
    {
        private readonly ILogger<TelegramController> _logger;
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramController(ILogger<TelegramController> logger, ITelegramBotClient telegramBotClient)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null)
            {
                _logger.LogWarning("Received null update");
                return Ok();
            }

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                _logger.LogInformation($"Received message: {message.Text}");
                await _telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"You said: {message.Text}");
            }
            return Ok();
        }

    }
}
