namespace duka;

public interface Iproduct
{
    Task<List<Product>> GetProducts();
    Task<List<Product>> GetProductsByCategoryId(Guid categoryId);
    Task<List<Product>> GetProductsByCategory(string categoryIdentifier);
    Task<Product> GetProduct(Guid Id);
    Task<string> AddProduct (Product newProduct);
    Task<string> UpdateProduct(Guid Id,Product updatedProduct);
    Task<string> DeleteProduct(Product product);

}
