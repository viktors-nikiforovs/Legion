using LegionWebApp.Configuration;
using Microsoft.Extensions.Options;

namespace LegionWebApp.Services
{
	public class OpenAiService : IOpenAiService
	{
		private readonly OpenAiConfiguration _openAiConfig;

		public OpenAiService(IOptionsMonitor<OpenAiConfiguration> optionsMonitor) {

			_openAiConfig = optionsMonitor.CurrentValue;
		}

		public async Task<string> CompleteSentence(string text)
		{
			var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
			var result = await api.Completions.GetCompletion(text);
			return result;
		}

		public async Task<string> TranslateText(string lang, string text)
		{
			var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);

			var chat = api.Chat.CreateConversation();

			chat.AppendSystemMessage($"You are translater AI. Translate this text into {lang}. Do not add any more text to it, try to be as professional and grammar correct as you can. You do not anwser me, just translate. If you cannot translate, do not tell me about it.");
			
			chat.AppendUserInput(text);

			var responce = await chat.GetResponseFromChatbotAsync();

			return responce;

		}
	}
}
