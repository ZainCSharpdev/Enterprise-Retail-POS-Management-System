using POSbackend.DTO.Batch;
using POSbackend.Models;
using Microsoft.EntityFrameworkCore;
using POSbackend.Repository.Interface.Payment;

namespace POSbackend.Repository.implement.Payment
{
    public class PaymentRepo(PosdbContext _context) : IPaymentRepo
    {
        public async Task<IEnumerable<PaymentDto>> GetPaymentsBySaleAsync(int saleId)
        {
            return await _context.Payments
                .Where(p => p.SaleId == saleId)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    SaleId = p.SaleId,
                    BillId = p.BillId,
                    Amount = p.Amount,
                    Method = p.Method,
                    Status = p.Status,
                    CreatedAt = (DateTime)p.CreatedAt
                })
                .ToListAsync();
        }

        // Add a new payment
        public async Task<PaymentDto?> AddPaymentAsync(PaymentDto payment)
        {
            var newPayment = new Models.Payment
            {
                SaleId = payment.SaleId,
                BillId = payment.BillId,
                Amount = (decimal)payment.Amount,
                Method = payment.Method,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _context.Payments.AddAsync(newPayment);
            await _context.SaveChangesAsync();

            payment.PaymentId = newPayment.PaymentId;
            payment.Status = newPayment.Status;
            payment.CreatedAt = (DateTime)newPayment.CreatedAt;
            return payment;
        }

        // Update payment status (e.g., mark as Paid, Failed, Cancelled)
        public async Task<PaymentDto?> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return null;

            payment.Status = status;
            await _context.SaveChangesAsync();

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                SaleId = payment.SaleId,
                BillId = payment.BillId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                CreatedAt = (DateTime)payment.CreatedAt
            };
        }

        // Void payment instead of hard delete
        public async Task<bool> VoidPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            payment.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
