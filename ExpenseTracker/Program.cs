using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;
// Skapar en ny instans av en WebApplicationBuilder för att konfigurera och bygga webbapplikationen.
var builder = WebApplication.CreateBuilder(args);

// Lägger till MVC-kontroller med vyer som en service i applikationen.
// Detta gör det möjligt att använda MVC-mönstret för att bygga webbgränssnitt.
builder.Services.AddControllersWithViews();

// Konfigurerar Entity Framework och dess DbContext.
// Använder SQL Server som databas och hämtar anslutningssträngen från konfigurationsfilen.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Konfigurerar en rutt för MVC-kontroller. 
// Specificerar en standardkontroller och åtgärd samt möjliggör en valfri id-parameter i rutten.
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
