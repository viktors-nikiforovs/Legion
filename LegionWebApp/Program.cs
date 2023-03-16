using Telegram.Bot;
using LegionWebApp.Controllers;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using LegionWebApp.Services;

var builder = WebApplication.CreateBuilder(args);
var telegramBotConfigurationSection = builder.Configuration.GetSection(TelegramBotConfiguration.Configuration);
builder.Services.Configure<TelegramBotConfiguration>(telegramBotConfigurationSection);

var telegramBotConfiguration = telegramBotConfigurationSection.Get<TelegramBotConfiguration>();

builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    TelegramBotConfiguration? botConfig = sp.GetConfiguration<TelegramBotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });
builder.Services.AddScoped<UpdateHandlers>();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-GB"),
        new CultureInfo("uk"),
        new CultureInfo("fr"),
        new CultureInfo("de")
    };
    options.DefaultRequestCulture = new RequestCulture("en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.MapBotWebhookRoute<TelegramBotController>(route: telegramBotConfiguration.Route);
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

public class TelegramBotConfiguration
{
    public static readonly string Configuration = "TelegramBotConfiguration";

    public string BotToken { get; init; } = default!;
    public string HostAddress { get; init; } = default!;
    public string Route { get; init; } = default!;
    public string SecretToken { get; init; } = default!;
}
