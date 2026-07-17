using POSbackend.DTO.Sale;

namespace POSbackend.Service.Interface.Sales
{
    public interface ISalesService
    {
        Task<SaleDto?> CreateSaleAsync(SaleDto sale, List<SalesDetailsDto> details);
        Task<SaleDto?> UpdateSaleAsync(SaleDto sale, List<SalesDetailsDto> details);
        Task<SaleDto?> FinalizeSaleAsync(int saleId, string method, decimal discount, decimal tax);
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
    }
}
