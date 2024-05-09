using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    // Klassdefinition för 'Expense' som representerar en utgiftspost i systemet.
    public class Expense
    {
        // Egenskap för att unikt identifiera varje utgift. Detta fungerar som en primärnyckel i en databastabell.
        public int Id { get; set; }

        // Beskrivning av utgiften. Den är obligatorisk och får vara högst 100 tecken lång.
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        // Beloppet för utgiften. Måste vara ett positivt värde.
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        // Datum då utgiften gjordes. Använder attributet DataType för att specifiera att endast datumet lagras.
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // Kategori för utgiften. Kategorierna är definierade i enum 'ExpenseCategory'.
        [Required]
        public ExpenseCategory Category { get; set; }

        // En beräknad egenskap som returnerar datumet i ett kort datumformat.
        // Detta är inte lagrat i databasen utan genereras varje gång egenskapen efterfrågas.
        public string FormattedDate => Date.ToShortDateString();
    }

    // Enumeration för att definiera olika kategorier av utgifter.
    public enum ExpenseCategory
    {
        Mat,      // Kategori för matutgifter.
        Kläder,   // Kategori för klädutgifter.
        Övrigt    // Kategori för alla andra typer av utgifter.
    }
}
