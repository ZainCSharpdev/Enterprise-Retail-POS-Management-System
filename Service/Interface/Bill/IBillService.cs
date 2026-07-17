using POSbackend.DTO.Bill;

namespace POSbackend.Service.Interface.Bill
{
    public interface IBillService
    {
        Task<BillDto> CreateBillAsync(BillDto bill);
        Task<IEnumerable<BillDto>> GetBillsBySaleAsync(int saleId);
        Task<BillDto?> UpdateBillStatusAsync(int billId, string status);
    }
}
