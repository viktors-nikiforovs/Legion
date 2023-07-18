namespace LegionWebApp.Services
{
	public interface IOpenAiService
	{
		Task<string> CompleteSentence(string text);
		Task<string> TranslateText(string lang, string text);
	}
}
