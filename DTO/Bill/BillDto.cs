namespace POSbackend.DTO.Bill
{
    public class BillDto
    {
        public int BillId { get; set; }
        public int SaleId { get; set; }
        public string? BillImage { get; set; }
        public string? PdfUrl { get; set; }
        public DateTime InsertedDate { get; set; }
        public string Status { get; set; }   // "Open", "Paid", "Cancelled"
        public decimal Amount { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public long CustomerNumber { get; set; }
    }
}
