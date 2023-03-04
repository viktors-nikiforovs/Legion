using Telegram.Bot;

namespace LegionWebApp.Controllers
{
    public class TelegramBot
    {
        public async Task SendMessageAsync(string message)
        {
            string adminChannel = "-1001868111517";
            var bot = new TelegramBotClient("5479970688:AAGILa42a5lSXICKw0NF5frMlkVEhco6moQ");
            await bot.SendTextMessageAsync(
            chatId: adminChannel,
            text: message
            );
        }


    }
}
