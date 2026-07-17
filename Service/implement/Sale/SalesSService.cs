using POSbackend.DTO.Batch;
using POSbackend.DTO.Bill;
using POSbackend.DTO.Sale;
using POSbackend.Repository.Interface.Bill;
using POSbackend.Repository.Interface.Payment;
using POSbackend.Repository.Interface.Sales;
using POSbackend.Service.Interface.Sales;

namespace POSbackend.Service.implement.Sale
{
    public class SalesSService(ISaleRepo _saleRepo, ISaleDetailRepo _saleDetailRepo, IPaymentRepo _paymentRepo, IBillRepo _billRepo) : ISalesService
    {
        public async Task<SaleDto?> CreateSaleAsync(SaleDto sale, List<SalesDetailsDto> details)
        {
            return await _saleRepo.CreateSaleAync(sale, details);
        }

        public async Task<SaleDto?> UpdateSaleAsync(SaleDto sale, List<SalesDetailsDto> details)
        {
            return await _saleRepo.UpdateSaleAync(sale, details);
        }

        public async Task<SaleDto?> FinalizeSaleAsync(int saleId, string method, decimal discount, decimal tax)
        {
            // 1. Finalize sale totals
            var finalizedSale = await _saleRepo.FinalizeSaleAsync(saleId, method, discount, tax);
            if (finalizedSale == null) return null;

            // 2. Create payment record
            var payment = new PaymentDto
            {
                SaleId = saleId,
                Amount = finalizedSale.NetAmount,
                Method = method,
                Status = "Paid",
                CreatedAt = DateTime.Now
            };
            await _paymentRepo.AddPaymentAsync(payment);

            // 3. Generate bill
            var bill = new BillDto
            {
                SaleId = saleId,
                Amount = finalizedSale.NetAmount,
                Status = "Paid",
                InsertedDate = DateTime.Now,
                Name = finalizedSale.CustomerNumber.ToString(),
                Email = finalizedSale.CustomerNumber.ToString(),
                CustomerNumber = finalizedSale.CustomerNumber
            };
            await _billRepo.CreateBillAsync(bill);

            return finalizedSale;
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            return await _saleRepo.GetAllSalesAsync();
        }
    }
}
