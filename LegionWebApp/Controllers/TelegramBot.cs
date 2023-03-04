using Telegram.Bot;

namespace LegionWebApp.Controllers
{
    public class TelegramBot
    {
        public async Task SendMessageAsync(string message)
        {
            string adminChannel = "TELEGRAM_ADMIN_CHANNEL";
            var bot = new TelegramBotClient("TELEGRAM_BOT_TOKEN");
            await bot.SendTextMessageAsync(
            chatId: adminChannel,
            text: message
            );
        }
    }
}
