using LegionWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")));

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

var connectionString = builder.Configuration.GetConnectionString("herokypostgresql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Use Migrations

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add console logging
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();


app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "telegram",
        pattern: "telegram",
        defaults: new { controller = "Telegram", action = "Index" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.MapRazorPages();

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var webhookUrl = "https://beta.legion-foundation.org/api/telegram";
var webhookInfo = await botClient.GetWebhookInfoAsync();
if (!string.IsNullOrEmpty(webhookInfo.Url))
{
    await botClient.DeleteWebhookAsync();
}
await botClient.SetWebhookAsync(webhookUrl);
app.Run();
