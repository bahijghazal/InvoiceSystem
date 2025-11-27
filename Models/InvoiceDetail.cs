namespace InvoiceSystem.Models
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int InvoiceHeaderId { get; set; }
        public InvoiceHeader InvoiceHeader { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

}
