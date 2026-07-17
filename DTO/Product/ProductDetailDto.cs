namespace POSbackend.DTO.Product
{
    public class ProductDetailDto
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Price { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public decimal Unit_Price { get; set; }
        public string SetSize { get; set; }
        public long BatchNumber { get; set; }
    }
}
