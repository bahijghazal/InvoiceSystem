using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required, Phone]
        public string PhoneNumber { get; set; } = null!;

        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<InvoiceHeader> Invoices { get; set; } = new List<InvoiceHeader>();
    }

}
