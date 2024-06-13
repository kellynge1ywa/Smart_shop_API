
using Microsoft.EntityFrameworkCore;

namespace duka;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<Product> Products {get;set;}
    public DbSet<Category> Categories {get;set;}
    public DbSet<User> AppUsers {get;set;}
    public DbSet<Roles> Roles {get;set;}

}
