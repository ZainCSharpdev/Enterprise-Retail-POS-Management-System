using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSbackend.DTO.Product;
using POSbackend.Service.Interface.Category;

namespace POSbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryDto dto)
        {
            var category = await _service.AddCategoryAsync(dto);
            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _service.AllCategoriesAsync();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromBody] CategoryDto dto,int Id)
        {
            var category = await _service.UpdateCategoryAsync(dto, Id);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
