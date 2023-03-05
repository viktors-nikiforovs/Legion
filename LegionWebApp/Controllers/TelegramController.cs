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
        private long lastProcessedUpdateId = 0;
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            TelegramBotClient client = new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN"));

            if (update.Id > lastProcessedUpdateId)
            {
                lastProcessedUpdateId = update.Id;

                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    await client.SendTextMessageAsync(update.Message.From.Id, "answer");
                }
            }
            return Ok();
        }
    }
}
