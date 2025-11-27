namespace InvoiceSystem.Models
{
    public class InvoiceHeader
    {
        public int InvoiceHeaderId { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateOnly OrderDate { get; set; }

        public decimal TotalFee { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }

}
