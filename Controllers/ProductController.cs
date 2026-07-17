using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.Product;
using POSbackend.Service.Interface.Products;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductDetailDto detail)
        {
            var product = await _productService.AddProductsAsync(detail);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDetailDto detail)
        {
            var product = await _productService.UpdateProductsAsync(detail, id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductsAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
