using Microsoft.EntityFrameworkCore;
using template_web_api.Common.Data;
using template_web_api.Interfaces.IRepositories;
using template_web_api.Models.Entities;

namespace template_web_api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WeatherDbContext _context;

    public ProductRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
