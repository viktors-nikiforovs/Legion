using Telegram.Bot;
using LegionWebApp.Controllers;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using LegionWebApp.Services;
using LegionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.Extensions.Localization;
using LegionWebApp.Localization;
using LegionWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Features;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IFileUploadService, S3FileUploadService>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(connectionString));

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
builder.Services.AddSingleton<IStringLocalizerFactory, PostgreSqlStringLocalizerFactory>();

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
builder.Services.AddSignalR();
builder.Services.AddLogging(loggingBuilder =>
{
	loggingBuilder.ClearProviders();
	loggingBuilder.AddDebug();
	loggingBuilder.AddConsole();
	loggingBuilder.SetMinimumLevel(LogLevel.Trace);
});


builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 100000000; // Set the limit for multipart body length to 100MB
	options.MemoryBufferThreshold = Int32.MaxValue;
});


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 6;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestBodySize = null; // Set the limit to null for unlimited size or set a specific limit (in bytes)
});


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
//app.MapBotWebhookRoute<TelegramBotController>(route: telegramBotConfiguration.Route);
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}"
//);

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}"
	);
	endpoints.MapRazorPages();
});

// Map a route to your SignalR hub
app.MapHub<ProgressHub>("/progressHub");



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

public class S3Settings
{
	public string Token { get; set; }
	public string Secret { get; set; }
	public string BucketName { get; set; }
	public string ServiceURL { get; set; }
}
