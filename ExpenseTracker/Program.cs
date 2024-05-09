using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
// Skapar en ny instans av en WebApplicationBuilder f�r att konfigurera och bygga webbapplikationen.
var builder = WebApplication.CreateBuilder(args);

// L�gger till MVC-kontroller med vyer som en service i applikationen.
// Detta g�r det m�jligt att anv�nda MVC-m�nstret f�r att bygga webbgr�nssnitt.
builder.Services.AddControllersWithViews();

// Konfigurerar Entity Framework och dess DbContext.
// Anv�nder SQL Server som databas och h�mtar anslutningsstr�ngen fr�n konfigurationsfilen.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Konfigurerar en rutt f�r MVC-kontroller. 
// Specificerar en standardkontroller och �tg�rd samt m�jligg�r en valfri id-parameter i rutten.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expenses}/{action=Index}/{id?}");

app.Run();
