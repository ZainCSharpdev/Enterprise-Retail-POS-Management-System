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
    }
}
