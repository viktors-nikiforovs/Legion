using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Filters;
using LegionWebApp.Services;
using Telegram.Bot.Types;

namespace LegionWebApp.Controllers;

public class TelegramBotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}
