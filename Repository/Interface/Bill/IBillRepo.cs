using POSbackend.DTO.Bill;
using POSbackend.Models;

namespace POSbackend.Repository.Interface.Bill
{
    public interface IBillRepo
    {
        Task<BillDto?> CreateBillAsync(Models.Bill bill);
        Task<IEnumerable<BillDto>> GetBillsBySaleAsync(int saleId);
        Task<BillDto?> UpdateBillStatusAsync(int billId, string status);
        Task<bool> VoidBillAsync(int billId);
    }
}
