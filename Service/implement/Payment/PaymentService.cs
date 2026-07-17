using POSbackend.DTO.Batch;
using POSbackend.Repository.Interface.Payment;
using POSbackend.Service.Interface.Payment;

namespace POSbackend.Service.implement.Payment
{
    public class PaymentService(IPaymentRepo _payRepo) : IPaymentService
    {
        public async Task<PaymentDto?> AddPaymentAsync(PaymentDto payment)
        {
            return await _payRepo.AddPaymentAsync(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsBySaleAsync(int saleId)
        {
            return await _payRepo.GetPaymentsBySaleAsync(saleId);
        }

        public async Task<PaymentDto?> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            return await _payRepo.UpdatePaymentStatusAsync(paymentId, status);
        }
    }
}
