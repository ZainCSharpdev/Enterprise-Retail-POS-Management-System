using POSbackend.DTO.Product;

namespace POSbackend.Service.Interface.Category
{
    public interface ICategoryService
    {
        Task<CategoryDto?> AddCategoryAsync(CategoryDto dto);
        Task<IEnumerable<CategoryDto>> AllCategoriesAsync();
        Task<CategoryDto?> UpdateCategoryAsync(CategoryDto dto,int Id);
        Task<bool> DeleteCategoryAsync(int Id);
    }
}
