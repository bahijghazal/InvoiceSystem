
    namespace InvoiceSystem.ViewModels
    {
        public class InvoiceEditVM
        {
            public int InvoiceHeaderId { get; set; }
            public int CustomerId { get; set; }
            public DateOnly OrderDate { get; set; }
            public string? LinesJson { get; set; }
        }
    }
