using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            // Skicka en lista av kategorier (nu baserade på enum) till vyn via ViewBag
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // Skicka tillbaka kategorierna om valideringen misslyckas
            ViewBag.CategoryList = new SelectList(System.Enum.GetValues(typeof(ExpenseCategory)));
            return View(expense);
        }

        public async Task<IActionResult> Index()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var expenses = await _context.Expenses
                .Where(expense => expense.Date.Month == currentMonth && expense.Date.Year == currentYear)
                .ToListAsync();

            // Beräknar den totala kostnaden för den aktuella månaden
            var totalCostThisMonth = expenses.Sum(expense => expense.Amount);

            // Skicka den totala kostnaden och listan av utgifter till vyn
            ViewBag.TotalCostThisMonth = totalCostThisMonth;

            return View(expenses);
        }

        // Andra actions...
    }
}

