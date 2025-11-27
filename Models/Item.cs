using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Stock { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }


}
