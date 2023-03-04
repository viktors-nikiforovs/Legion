using Telegram.Bot;

namespace LegionWebApp.Controllers
{
    public class TelegramBot
    {
        public async Task SendMessageAsync(string message)
        {
            string adminChannel = Environment.GetEnvironmentVariable("TELEGRAM_ADMIN_CHANNEL");
            var bot = new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN"));
            await bot.SendTextMessageAsync(
            chatId: adminChannel,
            text: message
            );
        }
    }
}
