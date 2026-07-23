using POSbackend.Repository.Interface.Product;
using POSbackend.Service.Interface.Products;
using POSbackend.DTO.Product;

namespace POSbackend.Service.implement.Product
{
    public class ProductService(IProductRepo _productRepo) : IProductService
    {
        public Task<IEnumerable<ProductDetailDto>> GetAllProductsAsync()
        {
            return _productRepo.GetAllProductsAsync();
        }
        public Task<ProductDetailDto?> GetProductByIdAsync(int productId)
        {
            return _productRepo.GetProductByIdAsync(productId);
        }
        public Task<SearchDto> SearchProductsAsync(string keyword)
        {
            return _productRepo.SearchProductsAsync(keyword);
        }
        public Task<ProductDetailDto> AddProductsAsync(ProductDetailDto details,IFormFile photoFile)
        {
            return _productRepo.AddProductsAsync(details,photoFile);
        }
        public Task<ProductDetailDto?> UpdateProductsAsync(ProductDetailDto details, int Id)
        {
            return _productRepo.UpdateProductsAsync(details, Id);
        }
        public Task<bool> DeleteProductsAsync(int productId)
        {
            return _productRepo.DeleteProductsAsync(productId);
        }
    }
}
