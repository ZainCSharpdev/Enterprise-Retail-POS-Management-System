using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.Batch;
using POSbackend.Service.Interface.Payment;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService _paymentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PaymentDto payment)
        {
            var created = await _paymentService.AddPaymentAsync(payment);
            return Ok(created);
        }

        [HttpGet("sale/{saleId}")]
        public async Task<IActionResult> GetBySale(int saleId)
        {
            var payments = await _paymentService.GetPaymentsBySaleAsync(saleId);
            return Ok(payments);
        }

        [HttpPut("{paymentId}/status")]
        public async Task<IActionResult> UpdateStatus(int paymentId, string status)
        {
            var updated = await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
