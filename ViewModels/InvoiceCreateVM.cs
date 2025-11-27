
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.ViewModels
{
    public class InvoiceCreateVM
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        public string LinesJson { get; set; } = null!;
    }
}
