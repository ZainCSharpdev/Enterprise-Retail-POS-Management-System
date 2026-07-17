namespace POSbackend.DTO.Batch
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int SaleId { get; set; }
        public int? BillId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }   // "Cash", "Card", "Online Billplz"
        public string Status { get; set; }   // "Pending", "Paid", "Failed", "Cancelled"
        public DateTime CreatedAt { get; set; }
        public string? GatewayTransactionId { get; set; }
    }
}
