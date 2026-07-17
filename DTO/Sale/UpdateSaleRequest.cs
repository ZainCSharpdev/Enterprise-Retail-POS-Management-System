namespace POSbackend.DTO.Sale
{
    public class UpdateSaleRequest
    {
        public SaleDto Sale { get; set; }
        public List<SalesDetailsDto> Details { get; set; }
    }
}
