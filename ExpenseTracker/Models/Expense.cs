using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public ExpenseCategory Category { get; set; }  // Uppdatera datatypen till enum

        public string FormattedDate => Date.ToShortDateString();
    }

    public enum ExpenseCategory
    {
        Mat,
        Kläder,
        Övrigt
    }
}
