
using Microsoft.EntityFrameworkCore;

namespace duka;

public class ProductServices : Iproduct
{
    private readonly AppDbContext _appDbContext;
    public ProductServices(AppDbContext appDbContext)
    {
        _appDbContext=appDbContext;
    }
    public async Task<string> AddProduct(Product newProduct)
    {
        _appDbContext.Products.Add(newProduct);
        await _appDbContext.SaveChangesAsync();
        return "Product added";
    }

    public async Task<string> DeleteProduct(Product product)
    {
        _appDbContext.Products.Remove(product);
        await _appDbContext.SaveChangesAsync();
        return "Product deleted";
    }

    public async Task<Product> GetProduct(Guid Id)
    {
        return await _appDbContext.Products.Where(product=> product.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _appDbContext.Products.ToListAsync();
    }

    public async Task<string> UpdateProduct(Guid Id,Product updatedProduct)
    {
        var product=await _appDbContext.Products.Where(product=> product.Id == Id).FirstOrDefaultAsync();
        if(product != null)
        {
            product.Name=updatedProduct.Name;
            product.ImageURL=updatedProduct.ImageURL;

            await _appDbContext.SaveChangesAsync();
            return "Product updated!!";
        }

        return "Product not found";
    }
}
