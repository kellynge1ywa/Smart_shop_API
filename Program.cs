using duka;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<Iproduct,ProductServices>();
builder.Services.AddScoped<Icategory,CategoryServices>();
builder.Services.AddScoped<IUser,UserServices>();
builder.Services.AddScoped<IJwt, JwtServices>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddDbContext<AppDbContext>(options=>{
    options.UseNpgsql(builder.Configuration.GetConnectionString("myConnections"));
});

builder.AddSwaggerExtension();
builder.AddAuth();

//add authorization
builder.AdminPolicy();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMigrations();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.Run();
