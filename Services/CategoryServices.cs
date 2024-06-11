
using Microsoft.EntityFrameworkCore;

namespace duka;

public class CategoryServices : Icategory
{
    private readonly AppDbContext _dbContext;
    public CategoryServices(AppDbContext dbContext)
    {
        _dbContext=dbContext;
    }
    public async Task<string> AddCategory(Category newCategory)
    {
        _dbContext.Categories.Add(newCategory);
        await _dbContext.SaveChangesAsync();
        return "Category added";
    }

    public async Task<string> DeleteCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return "Category deleted!!";
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category> GetCategory(Guid Id)
    {
        return await _dbContext.Categories.Where(category => category.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<string> UpdateCategory(Guid Id,Category updatedCategory)
    {
        var category= await _dbContext.Categories.Where(category=> category.Id == Id).FirstOrDefaultAsync();
        if(category != null){
         category.Name = updatedCategory.Name;
        category.ImageURL = updatedCategory.ImageURL;
        
        await _dbContext.SaveChangesAsync();
        return "Category updated!!";

        }
        return "Category not found";
    }
}
