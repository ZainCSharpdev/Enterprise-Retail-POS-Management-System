using POSbackend.DTO.Batch;

namespace POSbackend.Repository.Interface.Payment
{
    public interface IPaymentRepo
    {
        Task<IEnumerable<PaymentDto>> GetPaymentsBySaleAsync(int saleId);
        Task<PaymentDto?> AddPaymentAsync(PaymentDto payment);
        Task<PaymentDto?> UpdatePaymentStatusAsync(int paymentId, string status);
        Task<bool> VoidPaymentAsync(int paymentId);
    }
}
