using LegionWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//  options.UseSqlServer(connectionString));


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//  options.UseMySql(connectionString, new MySqlServerVersion(new Version(5,6,50)), options => options.EnableRetryOnFailure()));

builder.Services.AddDbContext<DbContext>(x =>
{

    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    string connStr;

    if (env == "Development")
    {
        connStr = builder.Configuration.GetConnectionString("DefaultConnection");


    }
    else
    {
        // Use connection string provided at runtime by Heroku.
        var connUrl = Environment.GetEnvironmentVariable("CLEARDB_DATABASE_URL");

        connUrl = connUrl.Replace("mysql://", string.Empty);
        var userPassSide = connUrl.Split("@")[0];
        var hostSide = connUrl.Split("@")[1];

        var connUser = userPassSide.Split(":")[0];
        var connPass = userPassSide.Split(":")[1];
        var connHost = hostSide.Split("/")[0];
        var connDb = hostSide.Split("/")[1].Split("?")[0];


        connStr = $"server={connHost};Uid={connUser};Pwd={connPass};Database={connDb}";



    }

    x.UseMySql(connStr, new MySqlServerVersion(new Version(5, 6, 50)));

});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UA}/{action=About}/{id?}");

app.MapRazorPages();

app.Run();
