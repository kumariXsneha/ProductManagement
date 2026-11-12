using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using System.Security.Policy;
var builder = WebApplication.CreateBuilder(args); //Creates a builder: an object that collects all settings and services before the app is built.
                                                  //Reads appsettings.json, then appsettings.Development.json
                                                  // Add services to the container.Reads the environment (Development) from launchSettings.json.
builder.Services.AddControllersWithViews(); //builder.Services	The app’s service container: a list of “things the app can provide when someone asks.” This is Dependency Injection (DI).

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build(); //Takes everything from the builder and creates the actual WebApplication object, called app

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); //Tells browsers “always use https for this site in the future.” A security feature for real servers.
}

app.UseHttpsRedirection();//If someone opens http:// (not secure), redirect them to https:// (secure).
app.UseStaticFiles();// use files cs bootstrap, fonts 

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*How URLs are read with this rule:

URL Controller  Action(method)	id
/	Home (default)	Index (default)	none
/Student	Student	Index (default)	none
/Student/Create	Student	Create	none
/Student/Edit/4	Student	Edit	4
/Home/Privacy	Home	Privacy	none */

app.Run();
