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

            var result = await _saleService.CreateSaleAsync(request.Sale, request.Details);

            if (result == null)
                return BadRequest("Could not create sale.");

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSaleRequest request)
        {
            var updated = await _saleService.UpdateSaleAsync(request.Sale, request.Details);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // FIX: Bound variables to the incoming JSON request body payload from React
        [HttpPost("finalize/{saleId}")]
        public async Task<IActionResult> Finalize(int saleId, [FromBody] FinalizeSaleRequest request)
        {
            if (request == null) return BadRequest("Checkout parameters cannot be null.");

            var finalized = await _saleService.FinalizeSaleAsync(saleId, request.Method, request.Discount, request.Tax);
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
    }

    // Ensure these DTO wrapper classes are accessible by the binding layer
    public class FinalizeSaleRequest
    {
        public string Method { get; set; } = "CARD";
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
    }
}