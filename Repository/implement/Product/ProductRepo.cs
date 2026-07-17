using POSbackend.DTO.Product;
using POSbackend.Models;
using Microsoft.EntityFrameworkCore;
using POSbackend.Repository.Interface.Product;

namespace POSbackend.Repository.implement.Product
{
    public class ProductRepo(PosdbContext _context) : IProductRepo
    {
        public async Task<IEnumerable<ProductDetailDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Batch)
                .Select(p => new ProductDetailDto
                {
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    ProductName = p.ProductName,
                    ManufacturerName = p.ManufacturerName,
                    TotalQuantity = p.TotalQuantity,
                    Price = p.Price,
                    ExpiryDate = p.ExpiryDate,
                    Unit_Price = p.UnitPrice,
                    SetSize = p.SetSize,
                    BatchNumber = (long)p.Batch.Batchnumber
                }).ToListAsync();
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductDetailDto
                {
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    ProductName = p.ProductName,
                    ManufacturerName = p.ManufacturerName,
                    TotalQuantity = p.TotalQuantity,
                    Price = p.Price,
                    ExpiryDate = p.ExpiryDate,
                    Unit_Price = p.UnitPrice,
                    SetSize = p.SetSize,
                    BatchNumber = (long)p.Batch.Batchnumber
                }).FirstOrDefaultAsync();
        }

        public async Task<SearchDto> SearchProductsAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(keyword))
                .Select(p => new SearchDto
                {
                    ProductID = p.ProductId,
                    ProductName = p.ProductName
                }).FirstOrDefaultAsync();
        }

        public async Task<ProductDetailDto> AddProductsAsync(ProductDetailDto dto)
        {

            var products = new Models.Product
            {
                ProductName = dto.ProductName,
                ManufacturerName = dto.ManufacturerName,
                TotalQuantity = dto.TotalQuantity,
                Price = dto.Price,
                ExpiryDate = dto.ExpiryDate,
                UnitPrice = dto.Unit_Price,
                SetSize = dto.SetSize,
                ImageUrl = dto.ImageUrl,
                Category = new Category { CategoryName = dto.CategoryName },
                Batch = new Batch { Batchnumber = dto.BatchNumber }
            };

            await _context.Products.AddAsync(products);
            await _context.SaveChangesAsync();

            return new ProductDetailDto
            {
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                ManufacturerName = products.ManufacturerName,
                TotalQuantity = products.TotalQuantity,
                Price = products.Price,
                ExpiryDate = products.ExpiryDate,
                Unit_Price = products.UnitPrice,
                SetSize = products.SetSize,
                ImageUrl = products.ImageUrl,
                CategoryName = products.Category.CategoryName,
                BatchNumber = (long)products.Batch.Batchnumber
            };
        }
        public async Task<ProductDetailDto?> UpdateProductsAsync(ProductDetailDto details, int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return null;
            product.ImageUrl = details.ImageUrl;
            product.ProductName = details.ProductName;
            product.ManufacturerName = details.ManufacturerName;
            product.TotalQuantity = details.TotalQuantity;
            product.Price = details.Price;
            product.ExpiryDate = details.ExpiryDate;
            product.UnitPrice = details.Unit_Price;
            product.SetSize = details.SetSize;
            product.Categoryid = await _context.Categories
                .Where(c => c.CategoryName == details.CategoryName)
                .Select(c => c.Categoryid)
                .FirstOrDefaultAsync();
            product.Batchid = await _context.Batches
                .Where(b => b.Batchnumber == (int)details.BatchNumber)
                .Select(b => b.Batchid)
                .FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return details;
        }

        public async Task<bool> DeleteProductsAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
