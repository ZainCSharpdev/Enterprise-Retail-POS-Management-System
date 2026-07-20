using POSbackend.DTO.Bill;

namespace POSbackend.Service.Interface.Bill
{
    public interface IBillService
    {
        Task<BillDto?> CreateBillAsync(int saleId);
        Task<IEnumerable<BillDto>> GetBillsBySaleAsync(int saleId);
        Task<BillDto?> UpdateBillStatusAsync(int billId, string status);
    }
}
