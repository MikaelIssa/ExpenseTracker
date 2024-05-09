using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    // Definierar en kontrollerklass som hanterar utgifter. Den härver från `Controller` klassen, vilket ger den tillgång till MVC-funktioner.
    public class ExpensesController : Controller
    {
        // Privat fält för att lagra en referens till databaskontexten.
        private readonly ApplicationDbContext _context;

        // Konstruktor som initialiserar databaskontexten via dependency injection.
        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Asynkron metod för att visa en lista med utgifter för den nuvarande månaden.
        public async Task<IActionResult> Index()
        {
            var currentMonth = DateTime.Now.Month; // Hämtar nuvarande månad.
            var currentYear = DateTime.Now.Year; // Hämtar nuvarande år.
            var expenses = await _context.Expenses
                .Where(expense => expense.Date.Month == currentMonth && expense.Date.Year == currentYear)
                .ToListAsync(); // Hämtar alla utgifter för nuvarande månad och år från databasen.

            // Sparar den totala kostnaden för månaden i en ViewBag för att användas i vyn.
            ViewBag.TotalCostThisMonth = expenses.Sum(expense => expense.Amount);
            return View(expenses); // Returnerar vyn tillsammans med listan av utgifter.
        }

        // Metod för att visa formuläret för att skapa en ny utgift.
        public IActionResult Create()
        {
            // Skapar en SelectList av möjliga kategorier för utgifter som skickas till vyn för att användas i ett dropdown-menyn.
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)));
            return View();
        }

        // Asynkron metod för att behandla inlämningen av formuläret för att skapa en ny utgift.
        [HttpPost]
        [ValidateAntiForgeryToken] // Skydd mot CSRF-attacker.
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid) // Kontrollerar om inlämnade data är giltiga.
            {
                _context.Add(expense); // Lägger till utgiften i databasen.
                await _context.SaveChangesAsync(); // Sparar ändringarna.
                return RedirectToAction(nameof(Index)); // Omdirigerar till indexmetoden.
            }
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)));
            return View(expense); // Returnerar samma vy om datan inte är giltig för nytt försök.
        }

        // Asynkron metod för att visa formuläret för att redigera en befintlig utgift.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Om inget id är angivet, returnera 404-sida.
            }

            var expense = await _context.Expenses.FindAsync(id); // Hämtar utgiften med angivet id från databasen.
            if (expense == null)
            {
                return NotFound(); // Om utgiften inte finns, returnera 404-sida.
            }
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)), expense.Category);
            return View(expense); // Returnerar vyn för att redigera utgiften.
        }

        // Asynkron metod för att behandla inlämningen av formuläret för att redigera en utgift.
        [HttpPost]
        [ValidateAntiForgeryToken] // Skydd mot CSRF-attacker.
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound(); // Kontrollerar att id matchar id för utgiften som redigeras.
            }

            if (ModelState.IsValid) // Kontrollerar om inlämnade data är giltiga.
            {
                try
                {
                    _context.Update(expense); // Uppdaterar utgiften i databasen.
                    await _context.SaveChangesAsync(); // Sparar ändringarna.
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound(); // Hanterar felsituationer som datakonflikter.
                    }
                    else
                    {
                        throw; // Kastar ett undantag vid andra fel.
                    }
                }
                return RedirectToAction(nameof(Index)); // Omdirigerar till indexmetoden.
            }
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)), expense.Category);
            return View(expense); // Returnerar samma vy om datan inte är giltig för nytt försök.
        }

        // Asynkron metod för att hantera borttagning av en utgift.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Om inget id är angivet, returnera 404-sida.
            }

            var expense = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id); // Hämtar den första utgiften som matchar det angivna id.
            if (expense == null)
            {
                return NotFound(); // Om utgiften inte finns, returnera 404-sida.
            }

            _context.Expenses.Remove(expense); // Tar bort utgiften från databasen.
            await _context.SaveChangesAsync(); // Sparar ändringarna.
            return RedirectToAction(nameof(Index)); // Omdirigerar till indexmetoden.
        }

        // Hjälpmetod för att kontrollera om en viss utgift existerar genom att söka efter dess id i databasen.
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id); // Returnerar sant om utgiften finns, annars falskt.
        }
    }
}

