using template_web_api.Models.Entities;

namespace template_web_api.Interfaces.IRepositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
}
