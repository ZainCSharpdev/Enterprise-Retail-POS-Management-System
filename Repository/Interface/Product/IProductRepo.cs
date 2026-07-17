using POSbackend.DTO.Product;

namespace POSbackend.Repository.Interface.Product
{
    public interface IProductRepo
    {
        Task<IEnumerable<ProductDetailDto>> GetAllProductsAsync();
        Task<ProductDetailDto?> GetProductByIdAsync(int productId);
        Task<SearchDto> SearchProductsAsync(string keyword);
        Task<ProductDetailDto> AddProductsAsync(ProductDetailDto details);
        Task<ProductDetailDto?> UpdateProductsAsync(ProductDetailDto details, int Id);
        Task<bool> DeleteProductsAsync(int productId);
    }
}
