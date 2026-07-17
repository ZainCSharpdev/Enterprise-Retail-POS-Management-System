using POSbackend.DTO.Product;
namespace POSbackend.Service.Interface.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetailDto>> GetAllProductsAsync();
        Task<ProductDetailDto?> GetProductByIdAsync(int productId);
        Task<SearchDto> SearchProductsAsync(string keyword);
        Task<ProductDetailDto> AddProductsAsync(ProductDetailDto details);
        Task<ProductDetailDto> UpdateProductsAsync(ProductDetailDto details, int Id);
        Task<bool> DeleteProductsAsync(int id);
    }
}
