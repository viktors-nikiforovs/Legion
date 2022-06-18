using LegionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.AspNetCore.Mvc.Razor;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPortableObjectLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
            {

                new CultureInfo("en"),
                new CultureInfo("de"),
                new CultureInfo("fr"),
                new CultureInfo("uk")
            };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


//builder.Services.AddRazorPages().AddViewLocalization();

builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);


var connectionString = builder.Configuration.GetConnectionString("herokypostgresql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/EN/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EN}/{action=Index}/{id?}");

app.Run();
