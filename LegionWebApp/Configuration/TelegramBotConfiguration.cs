namespace LegionWebApp.Configuration
{
	public class TelegramBotConfiguration
	{
		public static readonly string Configuration = "TelegramBotConfiguration";

		public string BotToken { get; init; } = default!;
		public string HostAddress { get; init; } = default!;
		public string Route { get; init; } = default!;
		public string SecretToken { get; init; } = default!;
	}

}
