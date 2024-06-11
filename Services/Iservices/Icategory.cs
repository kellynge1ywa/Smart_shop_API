namespace duka;

public interface Icategory
{
    Task<List<Category>> GetCategories ();
    Task<Category> GetCategory(Guid Id);
    Task<string> AddCategory(Category newCategory);
    Task<string> UpdateCategory(Guid Id,Category updatedCategory);
    Task<string> DeleteCategory (Category category);

}
