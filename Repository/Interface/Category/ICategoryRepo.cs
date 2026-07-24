using POSbackend.DTO.Product;

namespace POSbackend.Repository.Interface.Category
{
    public interface ICategoryRepo
    {
        Task<CategoryDto?>AddCategoryAsync(CategoryDto dto);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> UpdateCategoryAsync(CategoryDto dto,int CategoryId);
        Task<bool> DeleteCategoryAsync(int CategoryId);
    }
}
