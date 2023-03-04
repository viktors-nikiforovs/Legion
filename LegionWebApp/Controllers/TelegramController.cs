using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LegionWebApp.Controllers
{
    [Route("api/[controller]")]
    public class TelegramController : Controller
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null)
            {
                return Ok();
            }

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                await _telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"You said: {message.Text}");
            }

            return Ok();
        }
    }
}
