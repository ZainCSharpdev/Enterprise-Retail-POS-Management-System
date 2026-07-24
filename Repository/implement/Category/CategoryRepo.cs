using Microsoft.EntityFrameworkCore;
using POSbackend.DTO.Product;
using POSbackend.Models;
using POSbackend.Repository.Interface.Category;

namespace POSbackend.Repository.implement.Category
{
    public class CategoryRepo(PosdbContext _context) : ICategoryRepo
    {
        public async Task<CategoryDto?> AddCategoryAsync(CategoryDto dto)
        {
            var entry = await _context.Categories.AddAsync(new Models.Category
            {
                CategoryName = dto.CategoryName,
            });
            await _context.SaveChangesAsync();
            return new CategoryDto
            {
                CategoryId = entry.Entity.Categoryid,
                CategoryName = entry.Entity.CategoryName,
            };
        }

        public async Task<bool> DeleteCategoryAsync(int CategoryId)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            return await _context.Categories
                 .Select(p => new CategoryDto
                 {
                     CategoryName = p.CategoryName,
                 }).ToListAsync();
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(CategoryDto dto,int CategoryId)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            if(category == null) return null;

            category.CategoryName = dto.CategoryName;

            await _context.SaveChangesAsync();
            return new CategoryDto
            {
                CategoryId=category.Categoryid,
                CategoryName = category.CategoryName
            };
        }
    }
}
