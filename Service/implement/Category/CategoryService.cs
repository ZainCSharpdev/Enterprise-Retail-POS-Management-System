using POSbackend.DTO.Product;
using POSbackend.Repository.Interface.Category;
using POSbackend.Service.Interface.Category;

namespace POSbackend.Service.implement.Category
{
    public class CategoryService(ICategoryRepo _repo) : ICategoryService
    {
        public async Task<CategoryDto?> AddCategoryAsync(CategoryDto dto)
        {
            return await _repo.AddCategoryAsync(dto);
        }

        public async Task<IEnumerable<CategoryDto>> AllCategoriesAsync()
        {
            return await _repo.GetCategoriesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int Id)
        {
            return await _repo.DeleteCategoryAsync(Id);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(CategoryDto dto, int Id)
        {
            return await _repo.UpdateCategoryAsync(dto, Id);
        }
    }
}
