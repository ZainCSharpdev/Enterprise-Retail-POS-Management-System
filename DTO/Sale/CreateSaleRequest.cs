namespace POSbackend.DTO.Sale
{
    public class CreateSaleRequest
    {
        public SaleDto Sale { get; set; }
        public List<SalesDetailsDto> Details { get; set; }
    }
}
