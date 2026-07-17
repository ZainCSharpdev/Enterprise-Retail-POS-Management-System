using POSbackend.DTO.Sale;

namespace POSbackend.Repository.Interface.Sales
{
    public interface ISaleRepo
    {
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<SaleDto?> GetSaleByIdAsync(int saleId);
        Task<IEnumerable<SaleDto>> GetSalesByDateAsync(DateTime SaleDate);
        Task<IEnumerable<SaleDto>> GetSalesByCustomerAsync(long customerNumber);
        Task<IEnumerable<SaleDto?>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);


        Task<SaleDto?> CreateSaleAync(SaleDto sale, List<SalesDetailsDto> details);
        Task<SaleDto?> UpdateSaleAync(SaleDto sale, List<SalesDetailsDto> details);
        Task<bool> DeleteSaleAsync(int saleId);


        Task<SaleDto?> FinalizeSaleAsync(int saleId, string method, decimal discount, decimal tax);
    }
}
