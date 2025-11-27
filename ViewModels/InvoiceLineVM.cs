namespace InvoiceSystem.ViewModels
{
    public class InvoiceLineVM
    {
        public int? InvoiceDetailId { get; set; }   // used on edit
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

}
