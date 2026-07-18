using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.Sale;
using POSbackend.Service.Interface.Sales;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController(ISalesService _saleService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
        {
            var created = await _saleService.CreateSaleAsync(request.Sale, request.Details);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSaleRequest request)
        {
            var updated = await _saleService.UpdateSaleAsync(request.Sale, request.Details);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPost("finalize/{saleId}")]
        public async Task<IActionResult> Finalize(int saleId, string method, decimal discount, decimal tax)
        {
            var finalized = await _saleService.FinalizeSaleAsync(saleId, method, discount, tax);
            if (finalized == null) return NotFound();
            return Ok(finalized);
        }

        // ==========================================
        // ADDED: LINE ITEM (CART) OPERATIONS
        // ==========================================

        [HttpGet("details/{saleId}")]
        public async Task<IActionResult> GetSaleDetails(int saleId)
        {
            var details = await _saleService.GetSaleDetailsAsync(saleId);
            return Ok(details);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> AddSaleDetail([FromBody] SalesDetailsDto detail)
        {
            await _saleService.AddSalesDetailsAsync(detail);
            return Ok();
        }


        [HttpPut("detail")]
        public async Task<IActionResult> UpdateSaleDetail([FromBody] SalesDetailsDto detail)
        {
            if (detail == null) return BadRequest("Update body cannot be empty");
            var updated = await _saleService.UpdateSaleDetailAsync(detail);
            if (updated == null) return NotFound("Sale detail record not found");

            return Ok(updated);
        }

        [HttpDelete("detail/{id}")]
        public async Task<IActionResult> DeleteSaleDetail(int id)
        {
            var deleted = await _saleService.DeleteSaleDetailAsync(id);
            if (!deleted) return NotFound($"Sale line item with ID {id} not found.");

            return NoContent();
        }

        public class FinalizeSaleRequest
        {
            public string Method { get; set; } = "CARD";
            public decimal Discount { get; set; }
            public decimal Tax { get; set; }
        }
    }

}
