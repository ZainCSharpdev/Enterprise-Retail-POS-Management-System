namespace POSbackend.DTO.Sale
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public long CustomerNumber { get; set; }
        public string status { get; set; }
        public string Method { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime? CreatedDate { get; set; }

        public List<SalesDetailsDto> SaleDetails { get; set; } = new List<SalesDetailsDto>();
    }
}
