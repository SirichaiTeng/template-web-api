using template_web_api.Interfaces;
using template_web_api.Interfaces.IExternalServices;
using template_web_api.Interfaces.IRepositories;
using template_web_api.Models.Entities;

namespace template_web_api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IExternalApiService _externalApiService;

    public ProductService(IProductRepository productRepository, IExternalApiService externalApiService)
    {
        _productRepository = productRepository;
        _externalApiService = externalApiService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var localProducts = await _productRepository.GetAllAsync();
        var externalProducts = await _externalApiService.GetProductsAsync();

        return localProducts.Concat(externalProducts);
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}
