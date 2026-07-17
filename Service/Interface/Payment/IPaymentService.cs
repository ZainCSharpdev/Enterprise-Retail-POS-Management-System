using POSbackend.DTO.Batch;

namespace POSbackend.Service.Interface.Payment
{
    public interface IPaymentService
    {
        Task<PaymentDto?> AddPaymentAsync(PaymentDto payment);
        Task<PaymentDto?> UpdatePaymentStatusAsync(int paymentId, string status);
        Task<IEnumerable<PaymentDto>> GetPaymentsBySaleAsync(int saleId);
    }
}
