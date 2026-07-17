using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.Bill;
using POSbackend.Service.implement.Bill;
using POSbackend.Service.Interface.Bill;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController(IBillService _billService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BillDto bill)
        {
            var created = await _billService.CreateBillAsync(bill);
            return Ok(created);
        }

        [HttpGet("sale/{saleId}")]
        public async Task<IActionResult> GetBySale(int saleId)
        {
            var bills = await _billService.GetBillsBySaleAsync(saleId);
            return Ok(bills);
        }

        [HttpPut("{billId}/status")]
        public async Task<IActionResult> UpdateStatus(int billId, string status)
        {
            var updated = await _billService.UpdateBillStatusAsync(billId, status);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
