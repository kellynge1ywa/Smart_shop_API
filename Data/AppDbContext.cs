﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace duka;

public class AppDbContext:IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<Product> Products {get;set;}
    public DbSet<Category> Categories {get;set;}
    public DbSet<User> AppUsers {get;set;}

}
