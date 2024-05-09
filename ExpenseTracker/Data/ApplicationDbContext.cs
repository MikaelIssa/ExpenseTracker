using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Konstruktor som tar emot konfigurationsalternativ för DbContext. 
        // 'base(options)' anropar bas-klassens konstruktor med dessa inställningar.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet<Expense> representerar samlingen av alla 'Expense' enheter i databasen.
        // Genom att exponera en DbSet för en specifik typ, kan du utföra CRUD-operationer på de typerna.
        public DbSet<Expense> Expenses { get; set; }
    }
}
