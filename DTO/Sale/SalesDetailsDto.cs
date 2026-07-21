namespace POSbackend.DTO.Sale
{
    public class SalesDetailsDto
    {
        public int? SaleDetailId { get; set; }
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPerItem { get; set; }
        public decimal? TaxPerItem { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
