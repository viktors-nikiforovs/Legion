using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Net.Models;
using System.Linq;

namespace LegionWebApp
{
    public class DiscordBot
    {
        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = "OTc0NjkzOTQ4MTY3Njg0MjM3.GDnqbQ.1BJwzDuDnQaeY2YKts_7T-jxbCnnMtLFnslHC4",
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.GuildMemberUpdated += ChangedNickname;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { "/", "!" },
                EnableDms = true,
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands>();
            await Client.ConnectAsync();
            await Task.Delay(-1);

        }

        private async Task ChangedNickname(DiscordClient s, GuildMemberUpdateEventArgs e)
        {
            string[] nicknames = { "бздун", "Косой мой Бог", "Косой мой Хозяин", "Я раб Косого", "Могу выпердеть чушь", "высраться в ДБД", "Люблю выссаться на Мане", "говно", "говенка", "говноед", "говномес", "говночист", "говяга", "говнюк", "говняный беглец", "говна пирога", "глиномес", "изговнять", "гнида", "гнидас", "гнидазавр", "гниданидзе", "гондон", "гондольер", "даун", "даунитто", "дерьмо", "дерьмодемон", "дерьмище", "дрисня", "дрист", "дристануть", "обдристаться", "дерьмак", "дристун", "дрочила", "суходрочер", "дебил", "дебилоид", "дрочка за сура", "драчун", "задрот", "дцпшник", "елда", "елдаклык", "елдище", "жопа", "жопошник", "залупа", "залупинец", "засеря", "засранец", " Могу засрать ваш мозг", "защеканец", "изговнять", "идиот", "изосрать", "курва", "кретин", "кретиноид", "курвырь", "лезбуха", "лох", "минетчица", "мокрощелка", "мудак", "мудень", "мудила", "мудозвон", "мудацкая", "мудасраная дерьмопроелдина", "мусор", "педрик", "пердеж", "пердение", "пердельник", "пердун", "пидор", "пидорасина", "пидорормитна", "пидорюга", "педерастер", "педобратва", "дружки педигрипал", "писька", "писюн", "спидозный пес", "ссаная псина", "спидораковый", "срать", "спермер", "спермобак", "спермодун", "срака", "сракаборец", "сракалюб", "срун", "сука", "сучара", "сучище", "титьки", "трипер", "хер", "херня", "херовина", "хероед", "охереть", "пошел на хер", "хитрожопый", "хрен моржовый", "шлюха", "шлюшидзе" };
            Random rand = new Random();
            int index = rand.Next(nicknames.Length);
            string newNickname = nicknames[index];
            if (e.Member.Id == 175159000701206528 && !nicknames.Contains(e.Member.Nickname))
            {
                Console.WriteLine($"{e.Member.Username} получил ник {newNickname}");
                await e.Member.ModifyAsync(x => x.Nickname = newNickname);
            }
        }
        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}