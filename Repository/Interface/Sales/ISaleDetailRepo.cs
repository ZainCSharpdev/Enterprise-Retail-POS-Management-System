using POSbackend.DTO.Sale;

namespace POSbackend.Repository.Interface.Sales
{
    public interface ISaleDetailRepo
    {
        Task<IEnumerable<SalesDetailsDto>> GetSaleDetailsAsync(int saleId);
        Task AddSaleDetailAsync(SalesDetailsDto detail);
        Task<SalesDetailsDto?> UpdateSaleDetailAsync(SalesDetailsDto detail);
        Task<bool> DeleteSaleDetailAsync(int saleDetailId);
    }
}
